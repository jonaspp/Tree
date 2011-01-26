using System;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Collections.Generic;

namespace Tree.Configuration
{
    public class LoggerConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("appenders", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(AppenderCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public AppenderCollection Appenders
        {
            get
            {
                AppenderCollection appenders =
                    (AppenderCollection)base["appenders"];
                return appenders;
            }
        }
    }

    public class AppenderCollection : ConfigurationElementCollection
    {
        public AppenderCollection()
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
            return new AppenderElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((AppenderElement)element).Name;
        }

        public AppenderElement this[int index]
        {
            get
            {
                return (AppenderElement)BaseGet(index);
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

        new public AppenderElement this[string Name]
        {
            get
            {
                return (AppenderElement)BaseGet(Name);
            }
        }

        public int IndexOf(AppenderElement url)
        {
            return BaseIndexOf(url);
        }

        public void Add(AppenderElement url)
        {
            BaseAdd(url);
        }
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(AppenderElement url)
        {
            if (BaseIndexOf(url) >= 0)
                BaseRemove(url.Name);
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

    public class AppenderElement : ConfigurationElement
    {
        public AppenderElement()
        {
        }

        [ConfigurationProperty("name")]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
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
        [ConfigurationProperty("namespace")]
        public string Namespace
        {
            get
            {
                return (string)this["namespace"];
            }
            set
            {
                this["namespace"] = value;
            }
        }
        [ConfigurationProperty("pattern")]
        public string Pattern
        {
            get
            {
                return (string)this["pattern"];
            }
            set
            {
                this["pattern"] = value;
            }
        }
        [ConfigurationProperty("path")]
        public string Path
        {
            get
            {
                return (string)this["path"];
            }
            set
            {
                this["path"] = value;
            }
        }
    }
}