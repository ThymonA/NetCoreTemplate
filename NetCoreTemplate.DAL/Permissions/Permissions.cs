namespace NetCoreTemplate.DAL.Permissions
{
    using System;

    public static class Permissions
    {
        public static string GetActionKey(Module module, Type type, Action action)
        {
            var moduleName = Enum.GetName(typeof(Module), module).ToLower();
            var typeName = Enum.GetName(typeof(Type), type).ToLower();
            var actionName = Enum.GetName(typeof(Action), action).ToLower();

            return $"{moduleName}.{typeName}.{actionName}";
        }
    }
}
