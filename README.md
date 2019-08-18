### serverless_for_fun
---
Try easily create a lambda serverless WebAPI WITHOUT SLEEP. I've try lots of way to awake this lambda function after distributing to AWS while still *sleeps very well* now. So share to those developers who are also interested in awaking aws lambda serverless WebAPI.

*series of *code_for_fun* *

### Requirements
---
AspNetCore 2.1 or greater

### Installation
---
After clone, install all dependencies
```sh
$ dotnet restore
```

### Config your AwakeController
---

We have built-in AwakeController defined in config file (**default**: appsettings.json)

* default Type: `ServerlessSleepless.Host.Services.SelfAwakeService`

```sh
"ServerlessSleepless.Host.Services.SelfAwakeService": {
    "Enabled": true,
    "DelayStartInSecs": 1,
    "PeriodInSecs": 60
}
```

* pre-defined awkers 1~4

```sh
"ServerlessSleepless.Awaker.AccessS3.SelfAwakeService": {
    "Enabled": true,
    "BucketName": "test-serverless-sleepless"
  },
  "ServerlessSleepless.Awaker.SQSLoop.SelfAwakeService": {
    "Enabled": false,
    "PublishMessageOnStartup": true,
    "MaxNumberOfMessages": 10,
    "Region": "us-west-2",
    "ServiceUrl": "http://sqs.us-west-2.amazonaws.com",
    "QueueUrl": "https://sqs.us-west-2.amazonaws.com/xxxxx/serverless-awake-self",
    "DelayStartInSecs": 1,
    "PeriodInSecs": 60
  },
  "ServerlessSleepless.Awaker.BurstCPU.SelfAwakeService": {
    "Enabled": true,
    "MaxProcessCount": 1000000
  },
  "ServerlessSleepless.Awaker.BurstMEM.SelfAwakeService": {
    "Enabled": true,
    "MinMemSizeMB": 128,
    "MaxMemSizeMB": 512
  }
```


### Run App
---
``` csharp
dotnet ServerlessSleepless.Host.dll
```

Startup.cs
```csharp
        public void ConfigureServices(IServiceCollection services)
        {
			// ...            

            // Core Self-awake service
            services.AddSingleton<ISelfAwakeService>(new SelfAwakeService(Configuration,
                typeof(Awaker.AccessS3.SelfAwakeService).Assembly,
                typeof(Awaker.SQSLoop.SelfAwakeService).Assembly,
                typeof(Awaker.BurstCPU.SelfAwakeService).Assembly,
                typeof(Awaker.BurstMEM.SelfAwakeService).Assembly
            )); 
        }
```

   
### Create your own Awaker
---
You can easily create a new awaker, some hints are here:
 * Inherits from the class SelfAwakerServiceBase, completes its two virtual functions: `abstract InitializeIfEnabled()` and `abstract TryAwake()`.

 * Configure its section in appsettings.json. The `Enabled` keyword reservied for this toggle awaker on / off.

 * Project reference awaker's assembly in `ServerlessSleepless.Host` main project and passes its assemly name as `SelfAwakeService`'s 2nd array parameter. 

```

*Enjoy this **serverless_for_fun** project*
