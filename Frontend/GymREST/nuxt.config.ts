


// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2024-04-03',
  devtools: { enabled: true },
  modules: ["@pinia/nuxt", "@nuxt/ui", "@nuxt-alt/proxy"],
  imports: { dirs: ["types/*.ts"] },  
  runtimeConfig: {
    public: { gymApiUrl: "http://localhost:5034/api/" },
  },
});