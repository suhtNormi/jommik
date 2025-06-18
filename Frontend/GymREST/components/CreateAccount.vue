<template>
  <div class="background u-card">
    <img src="~/assets/images/background1.jpg" alt="Background Picture" class="full-screen-image"/>
    <div class="align-center max-w-md w-full space-y-6">
      <h1 class="text-center" style="font-size:3rem">Loo konto:</h1>
      <div class="rounded-md shadow-sm mt-8 space-y-6">
        <form class="max-w-md w-full space-y-4">
          <div>
            <input
              id="username"
              name="username"
              v-model="props.user.username"
              class="basic"
              type="text"
              placeholder="Kasutajanimi"
              style="color: #333;" />
          </div>
          <div>
            <input
              id="password"
              name="password"
              v-model="props.user.password"
              class="basic"
              type="password"
              placeholder="Parool"
              style="color: #333;" />
          </div>

          <div>
            <button
              @click.prevent="submitForm"
              class="register-btn"> 
              <span class="absolute left-0 inset-y-0 flex items-center pl-3"></span>
              Registreeri
            </button>
          </div>

          <div class="register-link">
            <p>
              Konto olemas? <router-link to="/Login" >Logi sisse</router-link>
            </p>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { User } from '~/types/user';
import { useAuthStore } from '~/stores/authStore';
import { storeToRefs } from 'pinia';
import { useRouter } from 'vue-router';

const { createAccount } = useAuthStore();
const authStore = useAuthStore();
const { user } = storeToRefs(authStore);

const props = defineProps<{ user: User }>();
const router = useRouter();

const submitForm = async () => {
    createAccount(props.user);
    props.user.username = '';
    props.user.password = '';
    router.push({ name: 'add-initialdata' });
};
</script>
<style scoped>

@import '../designs/loginForm.css';

</style>
