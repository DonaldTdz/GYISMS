export class MenuItem {
  displayName = '';
  permission = '';
  icon = '';
  route = '';
  childMenus: MenuItem[];
  acl = '';

  hide = false;

  constructor(
    displayName: string,
    permission: string,
    icon: string,
    route: string,
    childMenus: MenuItem[] = null,
    hide: boolean = false,
    acl: string = '',
  ) {
    this.displayName = displayName;
    this.permission = permission;
    this.icon = icon;
    this.route = route;
    this.hide = hide;
    this.acl = acl;

    if (childMenus) {
      this.childMenus = childMenus;
    } else {
      this.childMenus = [];
    }
  }
}
