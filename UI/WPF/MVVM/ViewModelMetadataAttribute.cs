using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

using Ks.Common.Controls;

using Page = Ks.Common.Controls.Page;
using Window = Ks.Common.Controls.Window;

namespace Ks.Common.MVVM
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ViewModelMetadataAttribute : Attribute
    {
        public ViewModelMetadataAttribute(Type ViewType)
        {
            this.ViewType = ViewType;
            Verify.True(typeof(Window).IsAssignableFrom(ViewType) | typeof(Page).IsAssignableFrom(ViewType), "ViewType must be a Window or a Page.");
        }

        public Type ViewType { get; }

        private bool _IsSingleInstance = false;
        private bool _IsSingleInstance_Set = false;

        public bool IsSingleInstance
        {
            get => this._IsSingleInstance;
            set
            {
                if (this._IsSingleInstance_Set)
                {
                    throw new InvalidOperationException("Cannot set the IsSingleInstance property multiple times.");
                }

                this._IsSingleInstance_Set = true;
                this._IsSingleInstance = value;
            }
        }

        private CNullable<Type> _KsApplicationType;

        public Type KsApplicationType
        {
            get => this._KsApplicationType.Value;
            set
            {
                if (this._KsApplicationType.HasValue)
                {
                    throw new InvalidOperationException("Cannot set the KsApplicationType property multiple times.");
                }

                this._KsApplicationType = value;
            }
        }
    }
}
