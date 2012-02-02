using System;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Collections.Generic;

namespace Tree.Configuration
{
    public class ContainerConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("collection", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ContainerCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public ContainerCollection Collection
        {
            get
            {
                ContainerCollection container =
                    (ContainerCollection)base["collection"];
                return container;
            }
        }
    }

    public class ContainerCollection : ConfigurationElementCollection
    {
        public ContainerCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ContainerElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((ContainerElement)element).Type;
        }

        public ContainerElement this[int index]
        {
            get
            {
                return (ContainerElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public ContainerElement this[string Type]
        {
            get
            {
                return (ContainerElement)BaseGet(Type);
            }
        }

        public int IndexOf(ContainerElement url)
        {
            return BaseIndexOf(url);
        }

        public void Add(ContainerElement url)
        {
            BaseAdd(url);
        }
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(ContainerElement url)
        {
            if (BaseIndexOf(url) >= 0)
                BaseRemove(url.Type);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
    }

    public class ContainerElement : ConfigurationElement
    {
        public ContainerElement()
        {
        }
        [ConfigurationProperty("type")]
        public string Type
        {
            get
            {
                return (string)this["type"];
            }
            set
            {
                this["type"] = value;
            }
        }

        [ConfigurationProperty("impl")]
        public string Impl
        {
            get
            {
                return (string)this["impl"];
            }
            set
            {
                this["impl"] = value;
            }
        }

        [ConfigurationProperty("state", IsRequired = false)]
        internal string State
        {
            get
            {
                return (string)this["state"];
            }
            set
            {
                this["state"] = value;
            }
        }


        public Dictionary<string, object> StateProperties
        {
            get
            {
                string raw = State;
                if (String.IsNullOrEmpty(raw))
                {
                    return null;
                }
                Dictionary<string, object> props = new Dictionary<string, object>();
                string[] array = raw.Split(',');
                foreach (string p in array)
                {
                    if (p.Contains("="))
                    {
                        int token = p.IndexOf('=');
                        string key = p.Substring(0, token);
                        string value = p.Substring(token + 1);
                        props.Add(key, value);
                    }
                }
                return props;
            }
        }
    }
}