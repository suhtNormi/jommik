import type { NitroFetchOptions, NitroFetchRequest } from "nitropack";

export const useApi = () => {
  const runtimeConfig = useRuntimeConfig();

  const customFetch = async <T>(
    url: string,
    options?: NitroFetchOptions<NitroFetchRequest>
  ) => {
    if (options?.body && typeof options.body === 'object') {
      options.body = JSON.stringify(options.body); 
    }
    const headers = {
      'Content-Type': 'application/json', 
      ...options?.headers,
    };

    return await $fetch<T>(url, {
      baseURL: runtimeConfig.public.gymApiUrl,
      headers,
      ...options,
      
    });
  };
  return { customFetch };
};
