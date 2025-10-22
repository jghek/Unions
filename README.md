# Unions

**Unions** provides minimal, allocation-free discriminated unions for C#, aligned with the [official C# unions language proposal](https://github.com/dotnet/csharplang/blob/main/proposals/unions.md).

It’s designed to be **drop-in compatible** with the upcoming native feature — giving you modern, idiomatic C# union types today.

---

## Why this exists

C# doesn’t yet support native union types, but the design is already defined in the official proposal.
This library implements that proposal’s shape and semantics with minimal overhead:

* Mirrors the syntax and behavior described in the C# language proposal.
* Easy to **adopt now** and **remove later** with minimal refactoring.
* Encourages **pattern matching** and standard C# flow control — no custom functional abstractions required.

---

## What’s included

`Union<T1, T2>` through `Union<T1, …, T9>`

Each union is an immutable `readonly struct` that supports:

* **Implicit conversion** from any of its case types (`T1` … `Tn`).
* **`TryGet(out Tn)`** to safely extract a specific case.
* **`Value`** exposing the contained object for pattern matching.
* **`ToString()`** forwarding to the stored value (or `"null"`).
* **Thread safety** and immutability — all members are readonly.

---

## Installation

Add **Unions** from NuGet:

```bash
dotnet add package Unions
```

Or via the Visual Studio Package Manager:

```powershell
Install-Package Unions
```

---

## Quick start

### Define a union alias (optional)

```csharp
using Pet = Union<Cat, Dog, Bird>;
```

### Construct values

```csharp
Pet pet = new Dog("Rex");
pet = new Cat("Millie");

// or using implicit conversions
Pet other = new Bird("Tweety");
```

### Extract values safely

```csharp
if (pet.TryGet(out Dog dog))
    Console.WriteLine($"Dog: {dog.Name}");
else if (pet.TryGet(out Cat cat))
    Console.WriteLine($"Cat: {cat.Name}");
else if (pet.TryGet(out Bird bird))
    Console.WriteLine($"Bird: {bird.Name}");
```

### Pattern match on `.Value`

```csharp
switch (pet.Value)
{
    case Dog d:
        Console.WriteLine($"Dog: {d.Name}");
        break;
    case Cat c:
        Console.WriteLine($"Cat: {c.Name}");
        break;
    case Bird b:
        Console.WriteLine($"Bird: {b.Name}");
        break;
    case null:
        Console.WriteLine("Pet is null");
        break;
}
```

### Returning unions from APIs

```csharp
Union<string, int> ParseOrEcho(string input)
    => int.TryParse(input, out var number) ? number : input;

var result = ParseOrEcho("42");

if (result.TryGet(out int n))
    Console.WriteLine($"Number: {n}");
else if (result.TryGet(out string s))
    Console.WriteLine($"Text: {s}");
```

---

## API overview

| Member                  | Description                                                                           |
| ----------------------- | ------------------------------------------------------------------------------------- |
| **Constructors**        | `new Union<T1,…>(Tn value)` for each supported type.                                  |
| **Implicit conversion** | Automatically converts any `Tn` into the union type.                                  |
| **TryGet**              | `bool TryGet(out Tn value)` safely extracts a case value.                             |
| **Value**               | `object? Value { get; }` exposes the stored value for inspection or pattern matching. |
| **ToString()**          | Delegates to the underlying value’s `ToString()` or returns `"null"`.                 |

---

## Behavior and semantics

### Nulls

* You can store `null` for any reference type:

  ```csharp
  Union<string, int> u = (string)null;
  ```
* `TryGet(out string s)` returns `true` and `s == null`.
* Pattern matching on `.Value` will correctly enter the `case null:` arm.
* Internally, a numeric *tag* tracks which type is active, so nulls are never ambiguous even when multiple `Tn` are reference types.

### Value types

* Value types are boxed internally (`object` field).
* Type-safe retrieval through `TryGet` prevents invalid casts.

### Immutability & thread safety

* All `Union` types are declared as `readonly struct`s.
* They are **fully immutable** after construction and safe for concurrent reads.

---

## Tips and idioms

* Use `using` aliases to name common unions in your domain:

  ```csharp
  using Amount = Union<int, decimal>;
  using Search = Union<User, Group, Organization>;
  ```

* Combine with switch expressions for concise logic:

  ```csharp
  int Size(Union<string, byte[]> data) => data.Value switch
  {
      string s => s.Length,
      byte[] b => b.Length,
      null => 0,
      _ => 0
  };
  ```

---

## Limitations

* Provided for **2–9 generic parameters** (`Union<T1,T2>` … `Union<T1,…,T9>`).
  The pattern can be extended if needed.
* No built-in equality or comparison operators (matches proposal minimalism).
* No custom serialization included — add converters as needed for JSON or binary serialization.
* Intentionally minimal: this is not a “Result” or “Option” monad library.

---

## Testing

Unit tests use **xUnit** with **FluentAssertions** and demonstrate usage patterns for all supported union types.

```bash
dotnet test
```

---

## License

Distributed under the `MIT license`. See the included `LICENSE` file for details.

---

## Links

* GitHub Repository: [github.com/jghek/Unions](https://github.com/jghek/Unions)
* C# Language Proposal: [dotnet/csharplang – Unions](https://github.com/dotnet/csharplang/blob/main/proposals/unions.md)
* NuGet Package: [Unions](https://www.nuget.org/packages/Unions)
