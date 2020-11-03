#include <Windows.h>
#include "PotatoLoader.h"
#include "PotatoLoader_Base.h"

BOOL WINAPI DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved)
{
	PotatoLoader::thisdll = hinstDLL;
	if (fdwReason == DLL_PROCESS_ATTACH)
	{
#ifndef DEBUG
		DisableThreadLibraryCalls(PotatoLoader::thisdll);
#endif
		PotatoLoader::Main();
	}
	else if (fdwReason == DLL_PROCESS_DETACH)
	{
		PotatoLoader::UNLOAD();
		FreeLibrary(PotatoLoader::thisdll);
	}
	return TRUE;
}