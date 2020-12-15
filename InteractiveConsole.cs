using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Ks.Common
{
    public abstract class InteractiveConsoleBase
    {
        public void Run()
        {
            Console.TreatControlCAsInput = true;
            this.OnLoad();
            while (true)
            {
                this.OnKeyPressed(Console.ReadKey(true));
            }
        }

        protected virtual void OnLoad()
        {
        }

        protected virtual void OnKeyPressed(ConsoleKeyInfo KeyInfo)
        {
        }
    }

    public abstract class InteractiveConsole : InteractiveConsoleBase
    {
        protected sealed override void OnKeyPressed(ConsoleKeyInfo KeyInfo)
        {
            if (KeyInfo.Modifiers != 0)
            {
                this.OnCombinationalKeyPressed(KeyInfo);
                return;
            }
            switch (KeyInfo.Key)
            {
                case ConsoleKey.Tab:
                    this.OnTabKeyPressed();
                    break;
                case ConsoleKey.Enter:
                    this.OnEnterKeyPressed();
                    break;
                default:
                    if (KeyInfo.KeyChar != default(char))
                    {
                        this.OnCharacterKeyPressed(KeyInfo);
                    }
                    else
                    {
                        this.OnOtherKeyPressed(KeyInfo);
                    }

                    break;
            }
        }

        protected virtual void OnEnterKeyPressed()
        {
        }

        protected virtual void OnTabKeyPressed()
        {
        }

        protected virtual void OnCombinationalKeyPressed(ConsoleKeyInfo KeyInfo)
        {
        }

        protected virtual void OnCharacterKeyPressed(ConsoleKeyInfo KeyInfo)
        {
        }

        protected virtual void OnOtherKeyPressed(ConsoleKeyInfo KeyInfo)
        {
        }
    }

    public class Scripter : InteractiveConsole
    {
        protected override void OnLoad()
        {
            Assembly.GetEntryAssembly();
        }

        private void AddAssemblyContainers(Assembly AssemblyInfo)
        {
            if (!this.Assemblies.Add(AssemblyInfo))
            {
                return;
            }

            foreach (var T in AssemblyInfo.GetTypes())
            {
                this.AddTypeContainers(new ComparableCollection<string>(T.FullName.Split('.', '+')), T);
            }

            foreach (var A in AssemblyInfo.GetReferencedAssemblies())
            {
                this.AddAssemblyContainers(Assembly.Load(A));
            }
        }

        private Container AddTypeContainers(ComparableCollection<string> Path, Type TypeInfo)
        {
            if (this.Containers.TryGetValue(Path, out var C))
            {
                Debug.Assert(C.TypeInfo == null | TypeInfo == null);
                if (C.TypeInfo == null)
                {
                    C.TypeInfo = TypeInfo;
                }

                return C;
            }

            var PathC = Path.Clone();
            C = this.Containers[PathC];

            C.Path = PathC;
            C.TypeInfo = TypeInfo;

            Path.RemoveAt(Path.Count - 1);
            C.Parent = this.AddTypeContainers(Path, null);
            C.Parent.Children.Add(C.Name, C);

            if (this.Names.ContainsKey(C.Name))
            {
                this.Names[C.Name] = Container.Ambiguous;
            }
            else
            {
                this.Names[C.Name] = C;
            }

            return C;
        }

        protected override void OnEnterKeyPressed()
        {
        }

        protected override void OnTabKeyPressed()
        {
        }

        protected override void OnCharacterKeyPressed(ConsoleKeyInfo KeyInfo)
        {
        }

#pragma warning disable IDE0052 // Remove unread private members
        private readonly Dictionary<string, object> Objects = new Dictionary<string, object>();
        private readonly System.Text.StringBuilder Input = new System.Text.StringBuilder();
#pragma warning restore IDE0052 // Remove unread private members
        private readonly SortedDictionary<string, Container> Names = new SortedDictionary<string, Container>();
        private readonly CreateInstanceDictionary<ComparableCollection<string>, Container> Containers = CreateInstanceDictionary.Create(new SortedDictionary<ComparableCollection<string>, Container>());
        private readonly HashSet<Assembly> Assemblies = new HashSet<Assembly>();

        public class Container
        {
            public string Name => this.Path[this.Path.Count - 1];

            public ComparableCollection<string> Path { get; set; }
            public Type TypeInfo { get; set; }
            public SortedDictionary<string, Container> Children { get; set; } = new SortedDictionary<string, Container>();
            public Container Parent { get; set; }

            public static readonly Container Ambiguous = new Container();
        }
    }
}
