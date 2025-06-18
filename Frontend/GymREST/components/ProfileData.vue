<template>
  <div>
    <h1 class="text-xl text-center font-bold">{{ title }}</h1>
    <div v-if="userData.length === 0" class="text-center">
      <p>Kasutajainfo puudub!</p>
    </div>

    <div v-else>
      <div class="flex flex-wrap mb-4">
        <UCard class="dark:bg-gray-800 text-gray-700 dark:text-gray-200 p-1 rounded-md shadow-sm max-w-[264px] flex flex-col items-center space-y-2 overflow-hidden">
          <h2 class="text-2xl font-bold mb-4">Profiilipilt:</h2>
          <div class="w-32 h-32 rounded-full bg-gray-300 flex items-center justify-center overflow-hidden">
            <img
              v-if="previewImageUrl"
              :src="previewImageUrl"
              alt="Profiilipilt"
              class="w-full h-full object-cover"
            />
            <div v-else class="w-32 h-32 rounded-full bg-gray-300 flex items-center justify-center">
              <p class="text-gray-500">Eelvaade</p>
            </div>
          </div>

          <input
            type="file"
            accept="image/png, image/jpeg"
            @change="onFileSelected"
            class="block w-full text-sm text-gray-600 file:mr-4 file:py-2 file:px-4 file:rounded-full file:border-0 file:bg-purple-100 file:text-purple-700 hover:file:bg-purple-200 mt-4"
          />
          <button
            @click="uploadFile" class="bg-purple-600 text-white px-4 py-2 rounded-md mt-4">
            Laadi pilt üles
          </button>
        </UCard>

        <div class="flex-1">
          <UTable :key="userData.length" :columns="columns" :rows="userData" />
          <div class="button-group mt-4">
            <router-link to="/add-initialdata" class="edit-btn">Muuda</router-link>
            <button class="delete-btn" @click="deleteUser">Kustuta</button>
          </div>
        </div>
      </div>

      <UCard class="dark:bg-gray-800 text-gray-700 dark:text-gray-200 p-6 rounded-lg shadow-md relative">
        <div v-if="selectedPlan" class="profile-plan space-y-4">
          <router-link to="/choose-plan" class="edit-btn bg-black text-white px-4 py-2 rounded absolute top-4 right-4">Vaheta kava</router-link>
          <div class="flex items-center">
            <div class="bg-purple-500 rounded-full p-2 mr-2 flex items-center justify-center">
              <div class="mt-4"> 
      </div>

              <component :is="getIcon(selectedPlan.goal)" class="text-white" />
            </div>
            <h2 class="text-2xl font-bold">{{ selectedPlan.title }}</h2>
          </div>

          <p class="text-lg text-gray-600">{{ selectedPlan.description }}</p>
         
          <div class="mt-2 text-gray-500">
            <p><strong>Eesmärk:</strong> {{ selectedPlan.goal }}</p>
            <p><strong>Level:</strong> {{ selectedPlan.level }}</p>
          </div>

          <h3 class="mt-4 font-semibold">Harjutused:</h3>
          <ul class="list-disc list-inside space-y-1">
            <li v-for="exercise in selectedPlan.exercises" :key="exercise.id" class="flex justify-between items-center">
            <div>
              {{ exercise.name }} - {{ exercise.sets }} seeriat ja {{ exercise.reps }} kordust
            </div>
            <UButton icon="i-heroicons-trash"  class="bg-purple-700 text-white hover:bg-purple-500"
              @click="deleteExercise(exercise.id)">
            </UButton>
          </li>
        </ul>

          <div class="mt-4">
            <UButton 
            v-if="!state.showInput" 
            @click="toggleInput"
            class="mt-4 bg-black text-white px-4 py-2 rounded-md hover:bg-gray-700">
            Lisa Harjutus
          </UButton>

      <div v-if="state.showInput" class="mt-2">
    <USelectMenu
    v-model="state.newExerciseName"
    name="exercise"
    :options="exercises"
    option-attribute="name"
    searchable
    creatable
  >
  <template #label>
      <template v-if="state.newExerciseName?.name">
        <span>{{  state.newExerciseName.name }} </span>
      </template>
      <template v-else>
        <span class="text-gray-500 dark:text-gray-400 truncate">Sisesta või vali harjutus</span>
      </template>
    </template>
  <template #option="{ option }">
      <span class="truncate">{{ option.name }}</span>
    </template>
  </USelectMenu>

            <!-- Sets Input -->
            <input 
              v-model.number="state.newExerciseSets" 
              type="number" 
              placeholder="Sisesta seeriate arv" 
              class="border rounded px-2 py-1 w-full mt-2"
              min="1"
              required />
            
            <!-- Reps Input -->
            <input 
              v-model.number="state.newExerciseReps" 
              type="number" 
              placeholder="Sisesta korduste arv" 
              class="border rounded px-2 py-1 w-full mt-2"
              min="1" 
              required />

            <!-- Save Button -->
            <button 
              @click="addExercise"
              class="mt-4 bg-black text-white px-4 py-2 rounded-md hover:bg-gray-700">
              Salvesta
            </button>
        </div>
    </div>
  </div>

  <div v-else>
    <p>Valitud plaan puudub.</p>
    <div class="mt-4"> 
      <router-link to="/choose-plan" class="choose-btn">Vali kava</router-link>
    </div>
        </div>
      </UCard>
    </div>
  </div>

  <!-- logout nupp -->
  <div id="app">
  <button 
    class="top-right-button" 
    @click="signOut">
    Logi välja
  </button>
  <div>
    <span class="absolute top-0 right-40 mt-4 mr-4 font text-2xl">Tere, {{ userData[0]?.name }}!</span>
</div>
</div>
<!-- v-if="isAuthenticated" peaks käima üles kui ma lõpüuks seda mõistan -->

</template>

<script setup lang="ts">
import { useUserStore } from '~/stores/stores';
import { defineProps } from 'vue';
import {onMounted } from 'vue';
import { ref } from 'vue';

defineProps<{ title: String }>();

const planStore = usePlanStore();
const { userData, updateUser, deleteUser, fetchUserData } = useUserStore();
const { selectedPlan, getIcon, addExercise, toggleInput, state, loadExercises, deleteExercise} = usePlanStore();
const { exercises } = storeToRefs(planStore);
const selectedExercise = ref<Exercise | undefined>(undefined);


//lisa
const auth = useAuthStore();
const { logout } = auth;
const { isAuthenticated } = storeToRefs(auth);
const router = useRouter();

onMounted(() => {
  fetchUserData();
});

onMounted(() => {
  loadExercises();
});

const signOut = () => {
    logout();
    router.push("/")
};

const columns = [
  { key: 'name', label: 'Kasutajanimi' },
  { key: 'age', label: 'Vanus' },
  { key: 'gender', label: 'Sugu' },
  { key: 'height', label: 'Pikkus (cm)' },
  { key: 'weight', label: 'Kaal (kg)' },
  { key: 'goal', label: 'Eesmärk' },
  { key: 'frequency', label: 'Treeningsagedus' },
  { key: 'level', label: 'Treeningtase' },
];
//eelvaade
const previewImageUrl = ref<string | null>(null);
const selectedFile = ref<File | null>(null);

const onFileSelected = (event: Event) => {
  const file = (event.target as HTMLInputElement).files?.[0];
  if (file && file.type.startsWith('image/')) {
    previewImageUrl.value = URL.createObjectURL(file);
    selectedFile.value = file;
  } else {
    previewImageUrl.value = null;
    alert('Vali kehtiv pildifail (PNG või JPEG)');
  }
};
const uploadFile = async () => {
  if (!selectedFile.value) {
    alert("Vali pilt, mida üles laadida!");
    return;
  }
const formData = new FormData();
formData.append("file", selectedFile.value); //fail vormi andmetesse

try {
  const response = await $fetch("/api/upload", {
    method: "POST",
    body: formData,
  });

  if (response && response.length > 0) {
    const profileImageUrl = response[0]; // server tagastab pildi URL-i
    alert("Pilt on üleslaetud!");
    const updatedUserData = { ...userData[0], profileImageUrl };
    await updateUser(updatedUserData);
  } else {
    alert("Pildi üleslaadimine ebaõnnestus!");
  }
} catch (error) {
  console.error("Tõrge üleslaadimisel:", error);
  alert("Tõrge üleslaadimisel.");
}};
</script>

<style scoped>
  @import '../designs/profile-btns.css';
</style>