using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace LumosLib
{
    public class DataManager : BaseDataManager
    {
        #region >--------------------------------------------------- INIT
        
        
        public override IEnumerator InitAsync()
        {
            yield return base.InitAsync();
            
            var tableLoader = GetLoader();
            if (tableLoader == null) yield break;

            tableLoader.SetPath(Project.Config.TablePath);

            yield return tableLoader.LoadJsonAsync();
            if (tableLoader.Json == "") yield break;
           
            
            var allSheets = JsonConvert.DeserializeObject<Dictionary<string, object[]>>(tableLoader.Json);

            var targetAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => 
                    a.GetName().Name == "Assembly-CSharp" || 
                    a.GetName().Name.StartsWith(GetType().Assembly.GetName().Name))
                .ToArray();
            
            var dataTypes = targetAssemblies
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); }
                    catch (ReflectionTypeLoadException e) { return e.Types.Where(t => t != null); }
                })
                .Where(t => typeof(BaseData).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

            
            foreach (var type in dataTypes)
            {
                if (allSheets.TryGetValue(type.Name, out var sheetJson))
                {
                    string sheetJsonStr = JsonConvert.SerializeObject(sheetJson);
                    var listType = typeof(List<>).MakeGenericType(type);
                    var list = (IList)JsonConvert.DeserializeObject(sheetJsonStr, listType);

                    var dict = new Dictionary<int, BaseData>();
                    
                    foreach (var item in list)
                    {
                        var data = (BaseData)item;
                        dict[data.ID] = data;
                    }
                    
                    _loadDatas[type] = dict;
                }
                else
                {
                    Project.PrintInitFail($" haven't sheet '{type.Name}'");
                }
            }
        }
         
        
        #endregion
        #region >--------------------------------------------------- GET

        
        private BaseDataLoader GetLoader()
        {
            return Project.Config.DataTableType switch
            {
                ProjectConfig.TableType.GoogleSheet => new GoogleSheetLoader(),
                _ => null,
            };
        }
        

        #endregion
    }
}