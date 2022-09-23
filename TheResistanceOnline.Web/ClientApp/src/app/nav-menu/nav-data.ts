export const navbarDataLoggedIn = [
  {
    // Home Component
    routeLink: '',
    icon: 'home',
    label: 'Home'
  },
  {
    // Game Component
    routeLink: 'the-resistance-game',
    icon: 'videogame_asset',
    label: 'Game'
  },
  //Account Component
  {
    routeLink: 'user/user-edit',
    icon: 'manage_accounts',
    label: 'Account'
  }
];

export const navbarDataNotLoggedIn = [
  {
    // Home Component
    routeLink: '',
    icon: 'home',
    label: 'Home'
  },
  {
    // Login Component
    routeLink: 'user/login',
    icon: 'login',
    label: 'Login'
  }
];

//todo put somewhere better
export interface SideNavToggle {
  screenWidth: number;
  collapsed: boolean;
}

