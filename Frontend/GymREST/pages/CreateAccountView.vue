<template>
  <div class="Layout">
    <CreateAccount :user="user" />
  </div>
  </template>
    
<script setup lang="ts">
import type { User } from '~/types/user';
import CreateAccount from '~/components/CreateAccount.vue';

const user: Ref<User> = ref<User>({
    username: '',
    password: '',
  });
</script>

<style scoped>
  @import '../designs/layout.css';
</style>