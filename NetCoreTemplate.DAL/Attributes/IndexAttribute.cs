namespace NetCoreTemplate.DAL.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class IndexAttribute : Attribute
    {
        public string Name { get; }

        public int Order { get; }

        public bool IsUnique { get; set; }

        public bool IsClustered { get; set; }

        public IndexAttribute()
            : this(string.Empty, -1)
        {
        }

        public IndexAttribute(string name)
            : this(name, -1)
        {
        }

        public IndexAttribute(string name, int order)
        {
            Name = name;
            Order = order;
        }
    }
}
