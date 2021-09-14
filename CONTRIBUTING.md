# Contributing

This project has adopted the [Microsoft Open Source Code of
Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct
FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com)
with any additional questions or comments.

For our general contributing guidelines please see [our dotnet/runtime contributing guide](https://github.com/dotnet/runtime/blob/master/CONTRIBUTING.md).

## Scope

This project's main aim is to provide a low-level infrastructure for reading and writing Word/Excel/PowerPoint documents. The project itself is very large due to all the specialized types to provide type-safety for accessing parts and elements. Below are the guidelines for the projects within the repo:

- `DocumentFormat.OpenXml`: This project is the core project. Its sole focus is on reading and writing the documents themselves.
  > For historical reasons, this project contains behavior that would otherwise fit in higher levels, but for now will be grandfathered in. As time goes on, these behaviors will probably be moved into other assemblies to more clearly stay in line with the layering described here.
- `DocumentFormat.OpenXml.Features`: A collection of features that build on top of the file format level and provide some nice to have features for quality of life improvements.
  > This may be scoped further to `DocumentFormat.OpenXml.Features.[DocumentType]` such as `DocumentFormat.OpenXml.Features.Word` as people will often be focused on a single type of document and won't care about the features for other document types.
- `DocumentFormat.OpenXml.Features.[FeatureName]`: This pattern will be used to provide features that may require a large amount of additional behavior or data to enable appropriately and wholly (such as `DocumentFormat.OpenXml.Linq` [to be renamed]).

Some questions that are considered when identifying where a new feature should go:

- Does this facillitate reading/writing the file format? `DocumentFormat.OpenXml`
- Does this provide functionality to aid developers but not necessarily scoped within the core SDK? `DocumentFormat.OpenXml.Features`
  > If there are internal APIs that are needed to enable features that build on top of the API, please open an issue to investigate how we can do that.

The decision to break the functionality into assemblies for layers are the following:

- Provide a more pay-for-play model. People should only carry the functionality they care about
- Enable scenarios like AOT better that do attempt to reduce unused pathways, but for best results require well-layered approaches

#### Open Questions

There are still some open questions regarding scoping, including the following:

- Do we want to scope things to document types? i.e. `DocumentFormat.OpenXml.Word`?
- Do we want to move strongly types types out into a separate layer?

## Prerequisites

The only prerequisite for building, testing, and deploying from this repository
is the [.NET SDK](https://get.dot.net/).
You should install the version specified in `global.json` or a later version within
the same major.minor.Bxx "hundreds" band.
See [.NET Core Versioning](https://docs.microsoft.com/en-us/dotnet/core/versions/) for more information.

The development experience is best with [Visual Studio][VisualStudio].

## Building

This repository can be built on Windows, Linux, and OSX.

Building, testing, and packing this repository can be done by using the standard dotnet CLI commands (e.g. `dotnet build`, `dotnet test`, `dotnet pack`, etc.).

Since there are a number of targets for the project, and loading all at once may cause slow performance in Visual Studio, the target framework can be controlled by an environment variable. This is controlled in [Directory.Build.props](./Directory.Build.props) via the environment variable `ProjectLoadStyle`. This changes over time, but that file will contain what the available load configurations are. By default, this will try to default to the current LTS version of .NET Core, but allows development against previous targets if needed. This is helpful, for instance, if you don't have the latest .NET installed.  The continuous integration system sets `ProjectLoadStyle=All` to build for all targets.

[VisualStudio]: https://docs.microsoft.com/dotnet/core/install/sdk?pivots=os-windows#install-with-visual-studio