<template>
  <div class="form-container">
    <UForm
      :validate="validate"
      :state="state"
      class="space-y-4"
      @submit ="onSubmit"
      @error="onError"
    >
    <h1 class="text-xl text-center">{{ title }}</h1>
      <UFormGroup label="Kasutajanimi" name="name">
        <UInput v-model="state.name" class="input" />
      </UFormGroup>

      <UFormGroup label="Vanus" name="age">
        <UInput v-model="state.age" type="number" min="1" max="100" step="1" class="input-f" />
      </UFormGroup>

      <UFormGroup label="Sugu" name="gender">
        <select v-model="state.gender" class="input-f">
          <option value="mees">Mees</option>
          <option value="naine">Naine</option>
        </select>
      </UFormGroup>

      <UFormGroup label="Pikkus" name="height" type="number">
        <input type="range" v-model="state.height" min="0" max="220" class="range-f"/>
        <p>{{ state.height }} cm</p>
      </UFormGroup>

      <UFormGroup label="Kaal" name="weight" type="number">
        <input type="range" v-model="state.weight" min="0" max="300" class="range-f" />
        <p>{{ state.weight }} kg</p>
      </UFormGroup>

      <UFormGroup label="Treeningute eesmärk" name="goal">
        <select v-model="state.goal" class="input-f">
          <option :value="TrainingGoal.Kaalulangetus">Kaalu langetamine</option>
          <option :value="TrainingGoal.Lihaskasv">Lihasmassi kasvatamine</option>
          <option :value="TrainingGoal.Vastupidavus">Vastupidavuse suurendamine</option>
          <option :value="TrainingGoal.Ülakeha">Suurendada ülakeha lihasmassi</option>
      </select>
      </UFormGroup>

      <UFormGroup label="Treeningsagedus" name="frequency">
        <input type="range" v-model="state.frequency" min="1" max="7" class="range-f" />
        <p>{{ state.frequency }} korda nädalas</p>
      </UFormGroup>

      <UFormGroup label="Treeningtase" name="level">
        <div class="radio-group">
          <label class="radio-label">
      <input 
        type="radio" 
        :value="TrainingLevel.Algaja" 
        v-model="state.level"
        name="level"  
      />
      {{ TrainingLevel.Algaja }}
    </label>
    <label class="radio-label">
      <input 
        type="radio" 
        :value="TrainingLevel.Kesktase" 
        v-model="state.level"
        name="level"
      />
      {{ TrainingLevel.Kesktase }}
    </label>
    <label class="radio-label">
      <input 
        type="radio" 
        :value="TrainingLevel.Edasijõudnud" 
        v-model="state.level"
        name="level"
      />
      {{ TrainingLevel.Edasijõudnud }}
    </label>
    </div>
  </UFormGroup>

      <div class="button-container">
        <UButton type="submit" class="submit-btn">{{ isEditMode ? 'Salvesta muudatused' : 'Salvesta' }}</UButton>
      </div>
    </UForm>
  </div>
</template>

<script setup lang="ts">

onMounted(() => {
   fetchUserData();
});

import { reactive, watchEffect, onMounted } from 'vue';
import { useUserStore } from '~/stores/stores';
import { useRouter } from 'vue-router';
import { TrainingGoal, TrainingLevel } from '~/types/enums';
defineProps<{ title: String }>();
import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
import type { InitialData } from "~/types/initialdata";
import type { ExtendedUser } from '~/types/extendedUser';

const { userData, addUser, updateUser, fetchUserData } = useUserStore();
const authStore = useAuthStore();
const router = useRouter();
//lisa----------------------------------
const { createProfile, updateInitialData } = useProfileStore();

const state = reactive<InitialData>({
  id: 0,
  name: '',
  age: 18,
  gender: '',
  height: 0,
  weight: 0,
  goal: null,
  frequency: 2,
  level: null,
  profileImageUrl: null
});

const user = reactive<ExtendedUser>({
  id: 0,
  username: '',
  password: ''
})

// lisa-----------------------------
onMounted(async () => {
  if (authStore.isAuthenticated) {
    await fetchUserData();
  }
});

watchEffect(() => {
  if (authStore.isAuthenticated === true && userData && userData.length > 0) {
    console.log("Kasutaja andmed:", userData.values);
    const user = userData[0];
    state.name = user.name;
    state.age = user.age;
    state.gender = user.gender;
    state.height = user.height;
    state.weight = user.weight;
    state.goal = user.goal;
    state.frequency = user.frequency;
    state.level = user.level;
  }
});
const isEditMode = computed(() => userData.length > 0); 

const validate = (state: any): FormError[] => {
  const errors = [];
  if (!state.name) errors.push({ path: "name", message: "Required" });
  if (state.age == 0) errors.push({ path: "age", message: "Required" });
  if (state.gender == '') errors.push({ path: "gender", message: "Required" });
  if (state.height == 0) errors.push({ path: "height", message: "Required" });
  if (state.weight == 0) errors.push({ path: "weight", message: "Required" });
  if (state.goal == null) errors.push({ path: "goal", message: "Required" });
  if (state.frequency == 0) errors.push({ path: "frequency", message: "Required" });
  if (state.level == null) errors.push({ path: "level", message: "Required" });
  return errors;
};


async function onSubmit(event: FormSubmitEvent<any>) {
  event.preventDefault();
  const errors = validate(state);
  if (errors.length > 0) {
    console.error('Vorm ei ole korrektne', errors);
    return;
  }
  try {
    if(isEditMode.value){
      console.log("updated successs");
      await updateInitialData(user.id,state);
      
    } else{
      await createProfile(user.id, state);
      
      console.log("created success");
    }
    router.push('/profile');

  } catch (error) {
    console.error('Viga vormi esitlemisel', error);
  }
}



async function onError(event: FormErrorEvent) {
  const element = document.getElementById(event.errors[0].id);
  element?.focus();
  element?.scrollIntoView({ behavior: "smooth", block: "center" });
}
</script>

<style scoped>
  @import '../designs/dataForm.css';
</style>
