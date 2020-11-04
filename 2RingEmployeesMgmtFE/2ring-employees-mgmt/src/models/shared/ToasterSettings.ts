export default class ToasterSettings {
  title: string = "";
  timer: number = 5;
  infiniteTimer: boolean = false;
  position: string = "topRight";
  showLeftIcn: boolean = true;
  showCloseIcn: boolean = true;
  type: string = "primary";

  constructor(obj?: any) {
    if (!obj) return;
    this.title = obj.title;
    this.timer = obj.timer;
    this.infiniteTimer = this.infiniteTimer;
    this.position = obj.position;
    this.showLeftIcn = obj.showLeftIcn;
    this.showCloseIcn = obj.showCloseIcn;
    this.type = obj.type;
  }
}
