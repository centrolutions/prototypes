using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ServiceMethodExportTester<T>
    {
        readonly List<ExportedMethodInfo> _ExportedMethods;

        public ServiceMethodExportTester()
        {
            _ExportedMethods = new List<ExportedMethodInfo>();
        }

        public ServiceMethodExportTester<T> ShouldExport(string methodName)
        {
            _ExportedMethods.Add(new ExportedMethodInfo(methodName));

            return this;
        }

        public ServiceMethodExportTester<T> WithParameter<TParam>(string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentNullException(nameof(parameterName));
            if (!_ExportedMethods.Any())
                throw new ArgumentOutOfRangeException($"Must call {nameof(ShouldExport)} before {nameof(WithParameter)}.");

            _ExportedMethods.Last().Parameters.Add(new ExportedParameterInfo(typeof(TParam), parameterName));

            return this;
        }

        public void AssertAll()
        {
            var allMethods = typeof(T).GetMethods();
            var errors = new List<string>();
            errors.AddRange(FindWronglyAttributedMethods(allMethods));
            errors.AddRange(FindMethodsWithWrongParameters(allMethods));

            if (errors.Count > 0)
                throw new Exception(string.Join(Environment.NewLine, errors));
        }

        IEnumerable<string> FindWronglyAttributedMethods(MethodInfo[] allMethods)
        {
            foreach (var method in allMethods)
            {
                if (!_ExportedMethods.Any(x => x.Name == method.Name))
                {
                    if (MethodHasExportAttribute(method))
                        yield return $"{method.Name} should not have the {nameof(ServiceMethodExportAttribute)} attribute.";
                }
                else
                {
                    if (!MethodHasExportAttribute(method))
                        yield return $"{method.Name} is missing the {nameof(ServiceMethodExportAttribute)} attribute.";
                }
                
            }
        }

        IEnumerable<string> FindMethodsWithWrongParameters(MethodInfo[] allMethods)
        {
            foreach (var exportedMethod in _ExportedMethods)
            {
                var actualMethod = allMethods.Single(x => x.Name == exportedMethod.Name);
                var actualParameters = actualMethod.GetParameters();
                var actualParameterNames = actualParameters.Select(x => x.Name);
                var expectedParameterNames = exportedMethod.Parameters.Select(x => x.Name);

                var missingParameterNames = expectedParameterNames
                    .Where(x => !actualParameterNames.Contains(x));

                var extraParameterNames = actualParameterNames
                    .Where(x => !expectedParameterNames.Contains(x));

                var misTypedParameterNames = actualParameters
                    .Where(x => exportedMethod.Parameters.Any(y => y.Name == x.Name && y.DataType != x.ParameterType))
                    .Select(x => x.Name);

                if (missingParameterNames.Any())
                    foreach (var pName in missingParameterNames)
                        yield return $"The paramter {pName} is missing from the {exportedMethod.Name} method.";

                if (extraParameterNames.Any())
                    foreach (var pName in extraParameterNames)
                        yield return $"The parameter {pName} was not expected on the {exportedMethod.Name} method.";

                if (misTypedParameterNames.Any())
                    foreach (var pName in misTypedParameterNames)
                        yield return $"The parameter {pName} on the {exportedMethod.Name} method has the wrong type.";
            }
        }

        bool MethodHasExportAttribute(MethodInfo method)
        {
            return method.GetCustomAttributes(typeof(ServiceMethodExportAttribute), false).Any();
        }



        class ExportedMethodInfo
        {
            public string Name { get; set; }
            public List<ExportedParameterInfo> Parameters { get; }

            public ExportedMethodInfo(string name)
            {
                Name = name;
                Parameters = new List<ExportedParameterInfo>();
            }
        }
        class ExportedParameterInfo
        {
            public Type DataType { get; set; }
            public string Name { get; set; }

            public ExportedParameterInfo(Type dataType, string name)
            {
                DataType = dataType;
                Name = name;
            }

        }
    }
}
