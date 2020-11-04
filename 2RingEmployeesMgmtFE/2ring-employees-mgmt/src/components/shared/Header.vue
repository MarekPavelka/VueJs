<template>
  <b-navbar toggleable="lg" type="light" variant="light">
    <b-navbar-brand id="navbar-home" to="/">
      <b-icon icon="house-fill"></b-icon>
      2ring
    </b-navbar-brand>
    <b-navbar-nav>
      <b-nav-item :to="currentEmployeesRoute">Current employees</b-nav-item>
      <b-nav-item :to="previousEmployeesRoute">Previous employees</b-nav-item>
      <b-nav-item
        :to="positionsRoute"
        :active="$route.name == 'availablePositions'"
        >Positions</b-nav-item
      >
    </b-navbar-nav>
    <b-collapse is-nav>
      <b-navbar-nav class="ml-auto">
        <b-nav-item-dropdown right>
          <template #button-content>
            <em>User</em>
          </template>
          <b-dropdown-item href="#">Profile</b-dropdown-item>
          <b-dropdown-item href="#">Sign Out</b-dropdown-item>
        </b-nav-item-dropdown>
      </b-navbar-nav>
    </b-collapse>
  </b-navbar>
</template>
<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import RouteData from "@/models/shared/RouteData";
import router from "@/router/index.ts";

@Component
export default class Header extends Vue {
  currentEmployeesRoute: any = { name: "currentEmployees" };
  previousEmployeesRoute: any = { name: "previousEmployees" };
  positionsRoute: any = { name: "availablePositions" };

  created() {}
}
</script>
