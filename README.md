

# Tazor

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Tazor is a NuGet package that seamlessly integrates Blazor Components to provide a dynamic and interactive static site generation solution. This allows you to harness the power of C# and the Blazor framework for building static websites with engaging user interfaces.

## Getting Started

### Installation

Install the Tazor NuGet package using the following command:

```bash
dotnet add package Tazor
```

### Usage

1. Update your project's `.csproj` file to use the `Microsoft.NET.Sdk.Razor` SDK:

    ```xml
    <Project Sdk="Microsoft.NET.Sdk.Razor">
        ...
    </Project>
    ```
2. Add the following code to your `Program.cs`:

    ```csharp
    using System;
    using System.Threading.Tasks;
    using Tazor;

    class Program
    {
        static async Task Main(string[] args)
        {
            await TazorGenerator.Generate();
        }
    }
    ```

3. Run your application:

    ```bash
    dotnet run
    ```

4. Your generated site will be available in the `Output` directory.

## Themes

Tazor supports theming with Blazor components, giving you the flexibility to create visually appealing and interactive themes for your site.

## Contributing

If you'd like to contribute to Tazor, please follow our [contribution guidelines](CONTRIBUTING.md). We welcome bug reports, feature requests, and pull requests.

## License

Tazor is licensed under the [MIT License](LICENSE.md).

## Support

For any questions or issues, check the [documentation](https://github.com/axologic/tazor) or open an [issue](https://github.com/axologic/tazor/issues).

Happy coding with Tazor!