<template>
  <div id="app">
    <div class="container">
      <app-header></app-header>
      <div class="container mt-3">
        <router-view></router-view>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import Header from "./components/shared/Header.vue";

@Component({
  components: {
    appHeader: Header,
  },
})
export default class App extends Vue {}
</script>

<style></style>
