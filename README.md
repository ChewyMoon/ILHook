# ILHook
Replace C# methods with methods of your own.

# Usage
```C#
public class Foo
{
    public void Bar()
    {
        Console.WriteLine("Bar!");
    }
}

public class FooHook
{
    public void HookFooBar()
    {
        var instance = new Foo();

        // Prints "Bar!"
        instance.Bar();

        var fooBarMethod = typeof(Foo).GetMethod("Bar", BindingFlags.Public | BindingFlags.Instance);
        var fooBarHook = typeof(FooHook).GetMethod(
            "FooBarReplacement",
            BindingFlags.Public | BindingFlags.Instance);

        fooBarMethod.ReplaceMethod(fooBarHook);

        // Prints "Foo!"
        instance.Bar();
    }

    public void FooBarReplacement()
    {
        Console.WriteLine("Foo!");
    }
}
```
