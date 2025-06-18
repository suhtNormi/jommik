<template>
  <div class="Main">
      <h1 class="jommik-txt">JÕMMIK</h1>
    <p v-if="!isAuthenticated" class="description-txt">
      <strong>Tere tulemast 'Jõmmik' treeningrakendusse!</strong> 
      <br />
      See on mugav viis jälgida oma edusamme ja progressi jõusaalis. 
      <br />
      Oled just alustanud või edasijõudnud ... sobib kõigile!
      <br />
      Jõmmikust leiad eesmärkidel põhinevad ja vastavalt tasemele treeningkavad.
      <br />
      Seega saad statistilise ülevaate enda füüsilise koormuse kohta.
      <br />
      Hakkas huvitama? Alustuseks täida ära algandmed, et leida just sulle sobiv treeningkava...
    </p>
    <p v-else class="description-txt">
      <strong>Tere tulemast 'Jõmmik' treeningrakendusse, olete sisse logitud!</strong> 
    </p>
      <img src="~/assets/images/background.jpg" alt="Background Picture" class="full-screen-image"/>
      <router-link v-if="!isAuthenticated" to="/CreateAccountView"> 
        <button class="start-btn">Registreeri</button>
      </router-link>
  </div>

  <router-link v-if="!isAuthenticated" to="/Login">
      <button class="login-btn">Logi sisse</button>
  </router-link>
  <button v-else @click="signOut" class="logOut-btn">Logi välja</button>
</template>

<script setup lang="ts">
import { useAuthStore } from '~/stores/authStore';
import { storeToRefs } from 'pinia'; 

const authStore = useAuthStore();
const { isAuthenticated } = storeToRefs(authStore); 
const { logout } = authStore; 

const signOut = () => {
logout(); 
};
</script>

<style scoped>
@import '../designs/indexPage.css';
</style>
