using System.Reactive.Disposables;

using Ks.Common;
using Ks.Common.MVVM;

using ReactiveUI;

//[[_
namespace _
{
    class _
    {
//]]_
//#!NamespaceClassStart

        private CompositeDisposable InitializeProperties()
        {
            var disposables = new CompositeDisposable();
            
            //[[Property
            this./*{*/HelperName/*}*/ = this./*{*/MethodName/*}*/().ToSafeProperty(this, nameof(this./*{*/Name/*}*/)).DisposeWith(disposables)!;
            //]]Property

            return disposables;
        }
        //[[Property

        /*{HelperAccessModifier}*/ ObservableAsPropertyHelper</*{*/Type/*}*/> /*{*/HelperName/*}*/ = ObservableAsPropertyHelper</*{*/Type/*}*/>.Default(DefaultValue.Get</*{*/Type/*}*/>(/*{*/DefaultValueTag/*}*/)!); // ToDo Remove the bang!
        //[[Attribute
        /*{Attribute}*/
        //]]Attribute
        /*{AccessModifier}*/ /*?Virtual{*/virtual /*}?*//*?New{*/new /*}?*//*{*/Type/*}*/ /*{*/Name/*}*/ => this./*{*/HelperName/*}*/.Value;
        //]]Property

//#!NamespaceClassEnd
//[[_
    }
}
//]]_
