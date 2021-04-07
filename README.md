# VR Client

![Test Suite](https://github.com/University-of-Kent-VR-Transport/vr-client/workflows/Test%20Suite/badge.svg)
[![codecov](https://codecov.io/gh/University-of-Kent-VR-Transport/vr-client/branch/master/graph/badge.svg?token=Y39H9MV6Y0)](https://codecov.io/gh/University-of-Kent-VR-Transport/vr-client)

## About

This is a final year group project for the module [CO600 at the University of Kent](https://www.kent.ac.uk/courses/modules/module/CO600). The project uses virtual reality to show realtime bus locations in selected areas.

## Getting Started

These instructions will get you a copy of the project up and running on your
local machine for development and testing purposes.

### Prerequisites

You'll need to install the following software:

```
Git v^2.0.0
Git Large File Storage (LFS) v^2.0.0
Unity v2019.4.12f1
```

#### Required Secrets

Mapbox needs a `MapboxConfiguration.txt` file located in
`/Assets/Resources/Mapbox`. The file should contain the following fields:
```
{
	"AccessToken":"your-mapbox-access-token",
	"MemoryCacheSize":500,
	"FileCacheSize":3000,
	"DefaultTimeout":30,
	"AutoRefreshCache":false
}
```

### Installation

A step by step series of examples that tell you how to get a development env
running.

Once the repo has been cloned. Open the project in Unity v2019.4.12f1.

## Built With

* [Unity](https://unity.com/) - A cross-platform game engine

## Versioning

We use [SemVer](https://semver.org/) for versioning. For the versions available,
see the
[tags on this repository](https://github.com/University-of-Kent-VR-Transport/vr-client/tags).

## Authors

* **Henry Brown** [HenryBrown0](https://github.com/HenryBrown0) `hb317@kent.ac.uk`
* **Joshua Lewis-Powell** [Wildcastle117](https://github.com/Wildcastle117) `jl715@kent.ac.uk`
* **Alex Fry** [the-dark-beat](https://github.com/the-dark-beat) `af491@kent.ac.uk`
