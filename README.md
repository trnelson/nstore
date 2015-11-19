<img src="http://travis.io/images/nstore-logo.png" width="300">

**NStore is a storage abstraction framework for .NET which simplifies and standardizes file operations in any cloud provider.**

Imagine a simple framework where, with just a few lines of code, you can save files to Azure Blobs, Amazon S3, or even to the local disk without worrying about details like authentication, containers, or cloud client creation. That's what NStore does, and I created it to abstract away the complexities of cloud file store APIs and SDKs and the nuances of saving files across multiple disparate providers.

NStore implements a simple API which allows you to perform simple CRUD (*create, read, update, delete*) operations to virtually any cloud provider. Included is an implementation for local file system, and I'm working on implementations for a few providers like Azure, Amazon S3, and more. I've included the core interfaces as an open-source SDK so that other developers can do the same.

NStore will eventually support `async` operations as well.

## How NStore Works

Following is a short example of how NStore works in practice. As you can see, the implementation is fast and easy. Configuration and authentication for cloud providers is handled automatically.

### Setup

Just call NStore's static `Init()` method and pass it a configuration string in XML format. A sample configuration file is provided. Using a `string` provides the opportunity for you to retrieve configuation data from a service or data store if you'd like and send it straight into NStore.

```
// Initialize NStore
NStore.Init(config);
```

That's it. NStore does the rest.

### Using NStore to Save to Local Disk

```
// Instantiate a repository (you'd do this in a constructor via Dependency Injection, probably)
var _repo = NStore.GetRepository<NStoreFileSystemRepository>();

// Get some data
var data = File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "example-files/mountain-fog.jpg");

// Create definitions
var file = new FileDefinition { Name = "output.jpg", Data = data };
var container = new ContainerDefinition { Path = @"C:/temp/photos" };

// Save file
_repo.Save(file, container);
```

And that's it. Actions like `Save()` also return a status, of course.

But what about other data stores, like Azure? 

### Using NStore to Save to Azure Block Blob Storage

By just changing the generic type for the repository factory, we can basically save to Azure's block blob storage with the same code (and a few other small changes, like specifying the container name and passing a relative path):

```
// Instantiate a repository (you'd do this in a constructor via Dependency Injection, probably)
var _repo = NStore.GetRepository<NStoreAzureBlockBlobRepository>();

// Get some data
var data = File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "example-files/mountain-fog.jpg");

// Create definitions
var file = new FileDefinition { Name = "output.jpg", Data = data };
var container = new ContainerDefinition { Name = "myphotos", Path = @"photos" };

// Save file
_repo.Save(file, container);
```

### Other CRUD Operations

Reading, deleting, or moving a file is just as easy. Configuration options allow you to specify whether to remove empty directories after deleting or moving files, but just like with saving, NStore uses a common interface for actions across all storage providers:

## All Storage Providers Work The Same

The benefit of having all storage providers work the same is that it's easy to switch providers and change very little code. Likewise, using multiple providers in a single application is dead simple too. Writing your own File Manager application or Social App? Why not integrate with NStore and get simple integration with a ton of cloud providers for free?

NStore is in early Alpha development and I'm very open to suggestions or contributions. Like what you see? Please use it, extend it, and let me know what you're doing with it!

License: MIT
