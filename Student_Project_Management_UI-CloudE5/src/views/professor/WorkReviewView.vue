<script setup>
import { ref, onMounted } from "vue";
import submissionService from "@/services/submissionService";
import DisplayWork from "@/components/DisplayWork.vue";
import { useRoute } from "vue-router";

const message = ref("");
const studentWorks = ref(null);
const route = useRoute();

const fetchStudentWork = async () => {
  const studentWorkId = route.query.id; 

  if (!studentWorkId) {
    message.value = "Student Work ID is missing.";
    return;
  }

  try {
    const response = await submissionService.getWorksForStudent(studentWorkId);

    if (response.status === 200) {
      studentWorks.value = response.data;

    } else {
      message.value = "Failed to fetch student work.";
    }
  } catch (error) {
    message.value = "Failed to retrieve student work.";
  }
};

onMounted(fetchStudentWork);


</script>

<template>
  <div>
    <h2>{{ route.query.username }} submissions</h2> 
    <p v-if="message" class="error">{{ message }}</p>
    <div v-if="studentWorks && !message && studentWorks.length">
        <ul>
        <li v-for="(work, index) in studentWorks" :key="index">
          <DisplayWork :studentWork="work" /> 
          ______________________________________________________
        </li>
      </ul>
    </div>
    <div v-else>Loading...</div>
    <div class="input-group">
    
  </div>
  </div>
</template>

