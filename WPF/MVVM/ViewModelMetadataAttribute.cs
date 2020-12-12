using System.Windows;
using System.Threading.Tasks;
using Mono;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Controls;
using System;
using System.Xml.Linq;
using Ks.Common.Controls;
using Window = Ks.Common.Controls.Window;
using Page = Ks.Common.Controls.Page;

namespace Ks.Common.MVVM
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ViewModelMetadataAttribute : Attribute
    {
        public ViewModelMetadataAttribute(Type ViewType)
        {
            this._ViewType = ViewType;
            Verify.True(typeof(Window).IsAssignableFrom(ViewType) | typeof(Page).IsAssignableFrom(ViewType), "ViewType must be a Window or a Page.");
        }

        private readonly Type _ViewType;

        public Type ViewType
        {
            get
            {
                return this._ViewType;
            }
        }

        private bool _IsSingleInstance = false;
        private bool _IsSingleInstance_Set = false;

        public bool IsSingleInstance
        {
            get
            {
                return this._IsSingleInstance;
            }
            set
            {
                if (this._IsSingleInstance_Set)
                    throw new InvalidOperationException("Cannot set the IsSingleInstance property multiple times.");
                this._IsSingleInstance_Set = true;
                this._IsSingleInstance = value;
            }
        }

        private CNullable<Type> _KsApplicationType;

        public Type KsApplicationType
        {
            get
            {
                return this._KsApplicationType.Value;
            }
            set
            {
                if (this._KsApplicationType.HasValue)
                    throw new InvalidOperationException("Cannot set the KsApplicationType property multiple times.");
                this._KsApplicationType = value;
            }
        }
    }
}
