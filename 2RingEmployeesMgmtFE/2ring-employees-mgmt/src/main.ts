import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import axios from "axios";

import { BootstrapVue, IconsPlugin } from "bootstrap-vue";
import "bootstrap/dist/css/bootstrap.css";
import "bootstrap-vue/dist/bootstrap-vue.css";

import VCalendar from "v-calendar";
import VueNotification from "@kugatsu/vuenotification";
import ToasterSettings from "@/models/shared/ToasterSettings";
import filters from "@/filters";

require("@/assets/css/global.css");

Vue.use(BootstrapVue);
Vue.use(IconsPlugin);

Vue.use(VCalendar);
Vue.use(VueNotification, new ToasterSettings());
Vue.use(filters);

Vue.config.productionTip = false;
axios.defaults.baseURL = "https://localhost:44376/";

new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount("#app");
