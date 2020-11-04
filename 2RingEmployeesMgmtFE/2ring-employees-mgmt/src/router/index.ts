import Vue from "vue";
import VueRouter, { RouteConfig } from "vue-router";
import Home from "@/components/Home.vue";
import CurrentEmployees from "@/components/employees/CurrentEmployees.vue";
import PreviousEmployees from "@/components/employees/PreviousEmployees.vue";
import AvailablePositions from "@/components/positions/AvailablePositions.vue";
import EmployeeDetail from "@/components/employees/shared/EmployeeDetail.vue";

Vue.use(VueRouter);

const routes: Array<RouteConfig> = [
  { path: "/", name: "Home", component: Home },
  {
    path: "/employees/current",
    name: "currentEmployees",
    component: CurrentEmployees,
  },
  {
    path: "/employees/current/add",
    name: "addEmployee",
    component: EmployeeDetail,
  },
  {
    path: "/employees/current/edit/:id",
    name: "editEmployee",
    component: EmployeeDetail,
    props: true,
  },
  {
    path: "/employees/archived",
    name: "previousEmployees",
    component: PreviousEmployees,
  },
  {
    path: "/employees/archived/:id",
    name: "previousEmployeeDetail",
    component: EmployeeDetail,
    props: true,
  },
  {
    path: "/positions",
    name: "availablePositions",
    component: AvailablePositions,
  },
  { path: "*", redirect: "/" },
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes,
});

export default router;
