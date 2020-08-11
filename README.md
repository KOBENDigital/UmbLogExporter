
# UmbLogExporter
A Log Viewer replacement for Umbraco 8

## What is UmbLogExporter?
UmbLogExporter builds on the Log Viewer that ships with the Umbraco 8 Core adding options to export logs to a format of your choosing. It also tweaks the Log Viewer interface allowing you to change the log date range when searching.

The default implementation exports to Excel format using the [EPPlus](https://www.nuget.org/packages/EPPlus/) nuget package.

## Install
Once installed, the `Log Viewer` node in the settings tree will be replaced with `Log Viewer/Exporter`.

You can export directly from the default page using the `View/Export` button groups next to each search:

<img src="./docs/images/umblogexporter-default.png" />

You can also export from the `Search` page using the `Search/Export` button group:

<img src="./docs/images/umblogexporter-search.png" />

The new date range field in the search bar allows for changing the dates without messing with the querystring or returning the default page to change the date span.

## Writing Your Own Exporters
The `UmbLogExporter.Core` project contains the `ILogExportBuilder` interface. The default implementation is `DefaultLogExportBuilder` which will give you a starting point for writing your own exporters.

Excel was a good option to ship with the project as most clients will be familiar with searching data in Excel. However, it could easily export to CSV, a database or a destination of your choosing.