<template>
  <div class="row">
    <div class="col-md-6" :class="{ 'offset-md-3': !isEditPage }">
      <b-overlay :show="isLoadingData" rounded="sm">
        <template #overlay>
          <div class="text-center">
            <b-icon
              icon="stopwatch"
              font-scale="3"
              animation="spin"
              variant="secondary"
            ></b-icon>
            <p id="cancel-label" class="text-secondary">Please wait...</p>
          </div>
        </template>
        <b-card
          id="card-employee"
          class="shadow-sm"
          border-variant="secondary"
          header-border-variant="secondary"
        >
          <template #header>
            <h2 class="text-center" v-if="isCreatePage">Add new employee</h2>
            <h2 class="text-center" v-else-if="isEditPage">
              Edit {{ employee | employeeFullName }}
            </h2>
            <h2 class="text-center" v-else>
              {{ employee | employeeFullName }}'s details
            </h2>
          </template>
          <b-form ref="employeeForm" novalidate @reset="onReset">
            <div class="form-group">
              <label>First name</label>
              <b-form-input
                :state="validity.FirstName"
                :disabled="isDetailPage"
                class="form-control"
                v-model="employee.FirstName"
              />
              <b-form-invalid-feedback>
                First name is required.
              </b-form-invalid-feedback>
              <b-form-valid-feedback> </b-form-valid-feedback>
            </div>
            <div class="form-group">
              <label>Surname</label>
              <b-form-input
                :state="validity.Surname"
                :disabled="isDetailPage"
                class="form-control"
                v-model="employee.Surname"
              />
              <b-form-invalid-feedback>
                Surname is required.
              </b-form-invalid-feedback>
              <b-form-valid-feedback> </b-form-valid-feedback>
            </div>
            <div class="form-group">
              <label>Address</label>
              <b-form-input
                :state="validity.Address"
                :disabled="isDetailPage"
                class="form-control"
                v-model="employee.Address"
              />
            </div>
            <b-form-invalid-feedback></b-form-invalid-feedback>
            <b-form-valid-feedback> </b-form-valid-feedback>
            <div class="form-group">
              <label>Birthday</label>
              <v-date-picker v-model="employee.Birthday">
                <template v-slot="{ inputValue, togglePopover }">
                  <div class="input-group">
                    <div class="input-group-prepend">
                      <b-button
                        variant="primary"
                        :disabled="isDetailPage"
                        type="button"
                        @click="togglePopover({ placement: 'bottom-start' })"
                      >
                        <b-icon icon="calendar2"></b-icon>
                      </b-button>
                    </div>
                    <b-form-input
                      :state="validity.Birthday"
                      disabled
                      class="form-control"
                      :value="inputValue"
                      placeholder="DD.MM.YYYY"
                    />
                    <b-form-invalid-feedback>
                      Birthday is required. Birthday has to be date from the
                      past.
                    </b-form-invalid-feedback>
                    <b-form-valid-feedback> </b-form-valid-feedback>
                  </div>
                </template>
              </v-date-picker>
            </div>

            <div class="form-group">
              <label> Start date</label>
              <v-date-picker v-model="employee.StartDate" v-if="!isEditPage">
                <template v-slot="{ inputValue, togglePopover }">
                  <div class="input-group">
                    <div class="input-group-prepend">
                      <b-button
                        :disabled="isDetailPage"
                        variant="primary"
                        type="button"
                        @click="togglePopover({ placement: 'bottom-start' })"
                      >
                        <b-icon icon="calendar2"></b-icon>
                      </b-button>
                    </div>
                    <b-form-input
                      disabled
                      :state="validity.StartDate"
                      class="form-control"
                      :value="inputValue"
                      placeholder="DD.MM.YYYY"
                    />
                    <b-form-invalid-feedback>
                      Start date is required. Start date can't be from the past.
                    </b-form-invalid-feedback>
                    <b-form-valid-feedback> </b-form-valid-feedback>
                  </div>
                </template>
              </v-date-picker>
              <div v-else class="form-control">
                {{ getDatePickerDateFormat(employee.StartDate) }}
              </div>
            </div>
            <div class="form-group">
              <label>Salary</label>
              <b-form-input
                :disabled="isDetailPage"
                :state="validity.Salary"
                type="number"
                v-model="employee.Salary"
              />
              <b-form-invalid-feedback>
                Salary is required.
              </b-form-invalid-feedback>
              <b-form-valid-feedback> </b-form-valid-feedback>
            </div>
            <div class="form-group">
              <label class="col-form-label">Position </label>
              <template v-if="!isDetailPage">
                <b-form-select
                  :state="validity.Position"
                  :disabled="isDetailPage"
                  v-model="selectedPositionId"
                  :options="availablePositions"
                  value-field="Id"
                  text-field="PositionName"
                >
                </b-form-select>
                <b-form-invalid-feedback
                  >Position is required</b-form-invalid-feedback
                >
                <b-form-valid-feedback> </b-form-valid-feedback>
              </template>
              <b-form-input
                v-else
                disabled
                :value="
                  employee.Position.Id == 0
                    ? 'N/A'
                    : employee.Position.PositionName
                "
              />
            </div>
            <div class="pt-4">
              <b-button
                type="button"
                pill
                variant="primary"
                class="d-flex ml-auto"
                @click="saveEmployee(employee)"
                v-if="!isDetailPage"
              >
                Save employee
              </b-button>
            </div>
          </b-form>
        </b-card>
      </b-overlay>
    </div>
    <div class="col-md-6" v-if="isEditPage">
      <b-card
        id="card-positionHistory"
        class="shadow-sm"
        border-variant="secondary"
        header-border-variant="secondary"
        align="center"
      >
        <template #header>
          {{ employee | employeeFullName }}'s position history
        </template>

        <table class="table">
          <thead>
            <tr>
              <th scope="col">Position Name</th>
              <th scope="col">Start Date</th>
              <th scope="col">End Date</th>
            </tr>
          </thead>
          <tbody>
            <template v-for="positionHistory in employee.PositionHistory">
              <tr
                v-if="positionHistory.Position !== null"
                :key="positionHistory.Id"
              >
                <td>
                  <div
                    class="text-primary"
                    v-if="positionHistory.EndDate == null"
                  >
                    {{ positionHistory.Position.PositionName }}
                  </div>
                  <div v-else>
                    {{ positionHistory.Position.PositionName }}
                  </div>
                </td>
                <td>{{ positionHistory.StartDate | filterDate }}</td>
                <td>
                  <div
                    class="text-primary"
                    v-if="positionHistory.EndDate == null"
                  >
                    current position
                  </div>
                  <div v-else>
                    {{ positionHistory.EndDate | filterDate }}
                  </div>
                </td>
              </tr>
              <tr v-else :key="positionHistory.Id">
                <td>
                  <div class="text-danger">
                    N/A
                  </div>
                </td>
                <td>{{ positionHistory.StartDate | filterDate }}</td>
                <td>{{ positionHistory.EndDate | filterDate }}</td>
              </tr>
            </template>
          </tbody>
        </table>
      </b-card>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import router from "@/router/index.ts";
import Employee from "@/models/Employee";
import Position from "@/models/Position";
import EmployeeService from "@/services/EmployeeService";
import PositionService from "@/services/PositionService";
import CurrentEmployees from "../CurrentEmployees.vue";
import axios, { AxiosResponse } from "axios";
import ToasterSettings from "@/models/shared/ToasterSettings";

type EmployeeValidity = { [index in keyof Employee]?: boolean | null };

@Component({
  components: {},
})
export default class EmployeeDetail extends Vue {
  @Prop() readonly id!: number;
  isLoadingData: boolean = false;
  isEditPage: boolean = false;
  isCreatePage: boolean = false;
  isDetailPage: boolean = false;

  availablePositions: Position[] = [];
  employee: Employee = new Employee();
  previousPositions: Position[] = [];

  formWasValidated: boolean = false;
  validity: EmployeeValidity = this.clearValidity();
  showForm: boolean = true;
  selectedPositionId: number = 0;

  created() {
    this.setComponentFlags();
    this.initializeEmployeeModel();
  }

  setComponentFlags(): void {
    if (router.currentRoute.name == "editEmployee") this.isEditPage = true;

    if (router.currentRoute.name == "addEmployee") this.isCreatePage = true;

    if (router.currentRoute.name == "previousEmployeeDetail")
      this.isDetailPage = true;
  }

  async initializeEmployeeModel(): Promise<void> {
    try {
      if (this.isCreatePage) {
        this.isLoadingData = true;
        const positions = await PositionService.getAllPositions();
        this.availablePositions = positions;
        this.selectedPositionId = this.availablePositions[0].Id;
        this.employee.Position = this.availablePositions[0];
        return;
      } else {
        const [positions, employee] = await Promise.all([
          PositionService.getAllPositions(),
          EmployeeService.getEmployee(this.id),
        ]);

        this.availablePositions = positions;
        this.employee = employee;
        this.selectedPositionId = this.employee.Position.Id;
      }
    } catch (error) {
      this.showErrorNotification(error);
    } finally {
      this.isLoadingData = false;
      console.log(this.employee.Position);
    }
  }

  async saveEmployee(): Promise<void> {
    this.employee.Position = this.availablePositions.filter(
      (p) => p.Id == this.selectedPositionId
    )[0];
    let isFormValid = this.validateForm();
    if (!isFormValid) {
      (<any>this).$refs.employeeForm.reset();
      return;
    }

    this.employee.Salary = parseFloat(<any>this.employee.Salary);
    this.isLoadingData = true;
    try {
      if (this.isCreatePage) {
        await EmployeeService.createEmployee(this.employee);
        this.employee = new Employee();
        this.selectedPositionId = this.availablePositions[0].Id;
      }
      if (this.isEditPage) {
        await EmployeeService.updateEmployee(this.employee);
        const updatedEmployee = await EmployeeService.getEmployee(
          this.employee.Id
        );
        this.employee = updatedEmployee;
      }
      this.validity = this.clearValidity();
      this.$notification.success("Employee saved successfully");
    } catch (error) {
      this.showErrorNotification(error);
    } finally {
      this.isLoadingData = false;
    }
  }

  getDatePickerDateFormat(date: Date) {
    return date.toLocaleDateString().replace(/ /g, "");
  }

  private validateForm(): boolean {
    this.validity.FirstName = this.employee.FirstName.length > 1;
    this.validity.Surname = this.employee.Surname.length > 1;
    this.validity.Address = true;
    this.validity.Birthday = this.validateBirthDay(this.employee.Birthday);
    this.validity.StartDate = this.validateStartDate(this.employee.StartDate);
    this.validity.Salary = this.employee.Salary > 0;
    this.validity.Position = this.employee.Position.Id != 0;

    const isFormValid = Object.entries(this.validity)
      .map(([key, value]) => value)
      .every((s) => s === true);

    return isFormValid;
  }

  private validateStartDate(startDate: Date | null): boolean {
    if (startDate == null) {
      return false;
    }
    const newDate = new Date();
    let isValid = true;
    if (this.isCreatePage) {
      isValid =
        startDate.getFullYear() >= newDate.getFullYear() &&
        startDate.getMonth() >= newDate.getMonth() &&
        startDate.getDate() >= newDate.getDate();
    }

    return isValid;
  }

  private validateBirthDay(birthDay: any): boolean {
    if (birthDay == null) return false;

    const isValid = birthDay <= new Date();
    return isValid;
  }

  private onReset(evt: any): void {
    evt.preventDefault();
    this.showForm = false;
    this.$nextTick(() => {
      this.showForm = true;
    });
  }

  private clearValidity(): EmployeeValidity {
    return {
      FirstName: null,
      Surname: null,
      Address: null,
      Birthday: null,
      StartDate: null,
      Salary: null,
      Position: null,
    };
  }

  private showErrorNotification(error: any): void {
    if (error.response) {
      this.$notification.error(error.response.data);
    } else {
      this.$notification.error(error.toString());
    }
  }
}
</script>
<style></style>
