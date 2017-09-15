using System;
using System.Runtime.InteropServices;

namespace SPC.LDAP.ProfileSync.Configuration
{
    [Guid("75d8cca0-753a-4c81-972a-6b51255adeb7")]
    public class PropertyMapping
    {
        public string Source { get; set; }
        public string RawValue { get; set; }

        public string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(RawValue))
                {
                    return "";
                }

                var parts = RawValue.Split(new[] { "==" }, StringSplitOptions.None);

                if (parts.Length == 2)
                {
                    return parts[1];
                }

                return "";
            }

            set
            {
                if (String.IsNullOrWhiteSpace(Destination))
                {
                    RawValue = "==" + value;
                }
                else
                {
                    RawValue = Destination + "==" + value;
                }
            }
        }

        public string Destination
        {
            get
            {
                if (string.IsNullOrWhiteSpace(RawValue))
                {
                    return "";
                }

                if (!RawValue.Contains("=="))
                {
                    return RawValue;
                }

                var parts = RawValue.Split(new[] { "==" }, StringSplitOptions.None);

                if (parts.Length >= 1)
                {
                    return parts[0];
                }

                return "";
            }

            set
            {
                if (String.IsNullOrWhiteSpace(Name))
                {
                    RawValue = value;
                }
                else
                {
                    RawValue = value + "==" + Name;
                }
            }

        }

        public bool IsValid
        {
            get { return (String.IsNullOrWhiteSpace(Source) == false && String.IsNullOrWhiteSpace(Destination) == false); }
        }

        public PropertyMapping(string source, string destination)
        {
            this.Source = source;
            this.RawValue = destination;
        }

        public PropertyMapping(string source, string destination, string name)
        {
            this.Source = source;
            this.Destination = destination;
            this.Name = name;
        }
    }
}
