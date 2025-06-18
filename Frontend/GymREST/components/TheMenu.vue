<template>
  <div class="navbar">
    <img src="~/assets/images/jommik.png" alt="Logo" class="logo" />
    <router-link v-for="link in filteredLinks" :key="link.label" :to="link.to">
      {{ link.label }}
    </router-link>
  </div>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { useRoute } from "vue-router";

const route = useRoute();
const authStore = useAuthStore();

const links = [
  {
    label: "Avaleht",
    to: "/",
  },
  {
    label: "Kalender",
    to: "/workout-calendar",
  },
  {
    label: "Tagasiside",
    to: "/silhouette",
  },
  {
    label: "Harjutuste andmed",
    to: "/WorkoutView",
  },
  {
    label: "Profiil",
    to: "/profile",
  },
  /*{
    label: 'login',
    to: "/Login",
  },
  {
    label: 'Register',
    to: '/CreateAccountView',
  }*/
];

const filteredLinks = computed(() => {
  if (!authStore.isAuthenticated) {
    return links.filter(link => link.label === "Avaleht");
  }
  return links;
});
</script>

<style scoped>
@import '../designs/indexPage.css';
</style>
