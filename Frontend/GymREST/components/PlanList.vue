<script setup lang="ts">
import { usePlanStore } from '~/stores/planStore';
import { useUserChosenPlanStore } from '~/stores/userChosenPlanStore';

const planStore = usePlanStore();
const userStore = useUserStore();
const { setSelectedPlan, loadPlans, getIcon } = planStore;
const router = useRouter();
const { plans } = storeToRefs(planStore);
const { userData }= storeToRefs(userStore);

//lisa
const {createUserChosenPlan, fetchLastUserFromProfile} = useUserChosenPlanStore();

onMounted(() => {
  //loadPlans();
  loadFilteredPlans();
});

  const viewPlans = computed(()=> plans.value.map(plan=> { return {
      ...plan,
      label: plan.title,
      key: plan.id,
  }}))
  
  const viewUser = computed(() => userData.value[0]);

const mapGoalToString = (goal: TrainingGoal | null): string => {
switch (goal) {
  case TrainingGoal.Kaalulangetus:
    return 'Kaalu%20langetamine'; // URL-encoded string
  case TrainingGoal.Lihaskasv:
    return 'Lihasmassi%20kasvatamine';
  case TrainingGoal.Vastupidavus:
    return 'Vastupidavuse%20suurendamine';
  case TrainingGoal.Ülakeha:
    return 'Suurenda%20ülakeha%20lihasmassi';
  default:
    return '';}
  };

  const loadFilteredPlans = async () => {
            if ( !viewUser.value?.goal || !viewUser.value?.level) {
              console.error('Goal or Level is not selected.');
              return;
            }

            const goal = mapGoalToString(viewUser.value.goal);
            const level = encodeURIComponent(viewUser.value.level.toString());

            const apiUrl = `http://localhost:5034/api/Plans/filter?goal=${goal}&level=${level}`;

            try {
              plans.value = await $fetch<Plan[]>(apiUrl);
              console.log(apiUrl);
              console.log('Fetched plans:', plans);
            } catch (error) {
              console.error('Failed to load filtered plans:', error);
            }
          };

const handleClick = async (plan: Plan) => {
  try {
    const profile = await fetchLastUserFromProfile();
    await createUserChosenPlan(profile.userId, plan.id);
    setSelectedPlan(plan);
    router.push('/profile');
  } catch (error) {
    console.error("couldnt fetch lastprofile or planid:", error);
  }
};






</script>
  <template>
         <div>
    <!-- Display the goal and level as a header -->
    <div v-if="viewUser" class="header bg-gray-200 p-4 rounded-md mb-4">
  <h2 class="text-lg font-bold text-gray-800">
    Treeningu eesmärk: {{ viewUser.goal }}  <br> Tase: {{ viewUser.level }}
  </h2>
</div>

  <div class="grid grid-cols-1 gap-4 md:grid-cols-2 lg:grid-cols-3">
    <div v-for="plan in plans" :key="plan.id" class="card bg-white p-6 rounded-lg shadow-md dark:bg-gray-800 dark:text-gray-200">

      <div class="flex items-center text-xl font-bold text-gray-800 dark:text-gray-100">
        <div class="bg-purple-500 rounded-full p-2 mr-2 flex items-center justify-center">
          <component :is="getIcon(plan.goal)" class="text-white" />
        </div>
        {{ plan.title }}
      </div>

      <p class="mt-2 text-sm text-gray-500 dark:text-gray-400">{{ plan.description }}</p>

      <div class="mt-4 space-y-3">
        <h4 class="font-bold text-lg">Harjutused:</h4>
        <ul class="list-disc list-inside">
          <li v-for="exercise in plan.exercises" :key="exercise.id">
            {{ exercise.name }} - {{ exercise.sets }} seeriat ja {{ exercise.reps }} kordust
          </li>
        </ul>
      </div>

      <button 
        @click="handleClick(plan)"
        class="mt-4 bg-black text-white px-4 py-2 rounded-md hover:bg-gray-700">
        Vali 
      </button>
    </div>
  </div>
</div>
</template>
