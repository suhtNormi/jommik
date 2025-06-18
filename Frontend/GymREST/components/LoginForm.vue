<template>
    <div class="background u-card" v-if="!isAuthenticated">
        <img src="~/assets/images/background1.jpg" alt="Background Picture" class="full-screen-image"/>
            <div class="align-center max-w-md w-full space-y-8">
                <h1 class="text-center" style="font-size:3rem">Logi sisse:</h1>
                <form @submit.prevent="submit">
                    <div class="rounded-md shadow-sm mt-8 space-y-6">
                        <div>
                            <input 
                                title="username" 
                                placeholder="Kasutajanimi" 
                                type="text" 
                                v-model="user.username"
                                name="username"
                                class="basic"
                                required
                                />
                               <i class="bx bxs-user"></i>
                               
                        </div>
                        <div>
                            <input 
                                title="password" 
                                placeholder="Parool" 
                                type="password" 
                                v-model="user.password"
                                name="password"
                                class="basic"
                                required
                                />
                               <i class="bx bxs-user"></i>
                        </div>

                        <div>
                            <button class="login-btn" type="submit">Logi sisse</button>
                       </div>

                        
                       <div class="register-link">
                      <p>
                      Pole kontot? <router-link to="/CreateAccountView">>Loo konto</router-link>
                      </p>
                      </div>
                      </div>
                      </form>
                <p v-if="showEmptyFieldsError" class="text-red-400">
                    Palun täitke kõik väljad
                </p>
                <p v-if="showError.valueOf()" class="text-red-400">
                    Vigane kasutajanimi või parool
                </p>
                
                
            </div>
    </div>

</template>

<script setup lang="ts">
import type { User } from '~/types/user';
import router from '~/router';
import { useAuthStore } from '~/stores/authStore';
import { storeToRefs } from '#build/imports';
import { ref } from '#build/imports';
import Login from '~/pages/Login.vue';

const auth = useAuthStore();
const user: User = { username: '', password: '' };
const showEmptyFieldsError = ref(false);

let showError = ref(false);

const submit = async () => {
    if (!user.username.trim() || !user.password.trim()) {
        showEmptyFieldsError.value = true;
        showError.value = false; 
        return;
    }
    showEmptyFieldsError.value = false;
    showError.value = !(await auth.login(user));
    
    if(!showError.value){
        router.push({ name: 'add-initialdata'}).then(() => {
            location.reload();
        })
    }
};

const { logout } = auth;
const { isAuthenticated } = storeToRefs(auth);

const signOut = () => {
    logout();
};
</script>
<style scoped>

@import '../designs/loginForm.css';

</style>