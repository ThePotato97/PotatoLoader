#include <Windows.h>
#include <string>
#include "PotatoLoader_Base.h"
#include "PotatoLoader.h"
#include "AssertionManager.h"
#include "Logger.h"
#include "HookManager.h"
#include "Il2Cpp.h"

bool PotatoLoader_Base::HasInitialized = false;
MonoMethod* PotatoLoader_Base::startup = NULL;

void PotatoLoader_Base::Initialize()
{
	AssertionManager::Start("PotatoLoader_Base.cpp", "PotatoLoader_Base::Initialize");
	if (Mono::Domain != NULL)
	{
		std::string modhandlerpath = std::string(PotatoLoader::GamePath) + "\\PotatoLoader\\PotatoLoader.ModHandler.dll";
		MonoAssembly* assembly = Mono::mono_domain_assembly_open(Mono::Domain, modhandlerpath.c_str());
		AssertionManager::Decide(assembly, "PotatoLoader.ModHandler.dll");
		if (assembly != NULL)
		{
			MonoImage* image = Mono::mono_assembly_get_image(assembly);
			AssertionManager::Decide(assembly, "Image");
			if (image != NULL)
			{
				MonoClass* klass = Mono::mono_class_from_name(image, "PotatoLoader", "PotatoLoaderBase");
				AssertionManager::Decide(assembly, "PotatoLoader.PotatoLoaderBase");
				if (klass != NULL)
				{
					MonoMethod* initialize = Mono::mono_class_get_method_from_name(klass, "Initialize", NULL);
					AssertionManager::Decide(initialize, "Initialize");
					if (initialize != NULL)
					{
						MonoObject* exceptionObject = NULL;
						Mono::mono_runtime_invoke(initialize, NULL, NULL, &exceptionObject);
						if ((exceptionObject != NULL) && PotatoLoader::DebugMode)
							Mono::LogExceptionMessage(exceptionObject);
						else
						{
							startup = Mono::mono_class_get_method_from_name(klass, "Startup", NULL);
							AssertionManager::Decide(startup, "Startup");
							if (PotatoLoader::IsGameIl2Cpp)
								HookManager::Hook(&(LPVOID&)Il2Cpp::il2cpp_runtime_invoke, HookManager::Hooked_runtime_invoke);
							else
								HookManager::Hook(&(LPVOID&)Mono::mono_runtime_invoke, HookManager::Hooked_runtime_invoke);
							HasInitialized = true;
						}
					}
				}
			}
		}
	}
}

void PotatoLoader_Base::Startup()
{
	if (startup != NULL)
	{
		MonoObject* exceptionObject = NULL;
		Mono::mono_runtime_invoke(startup, NULL, NULL, &exceptionObject);
		if ((exceptionObject != NULL) && PotatoLoader::DebugMode)
			Mono::LogExceptionMessage(exceptionObject);
	}
}