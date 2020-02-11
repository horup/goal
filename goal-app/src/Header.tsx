import * as React from 'react';
import { Toolbar, IconButton, Menu, MenuItem, LinearProgress, Typography, AppBar } from '@material-ui/core';
import MenuIcon from '@material-ui/icons/Menu';
import history from './history';
declare var process;

interface IProps
{
  refreshing:boolean;
}

export const Header = ({refreshing}:IProps)=>
{
    const [showMenu, setShowMenu] = React.useState<boolean>(false);

    const handleOpen = ()=>
    {
      setShowMenu(true);
    }
    const handleClose = (redirect:string)=>
    {
      setShowMenu(false);
      if (redirect.length > 0)
        history.push(redirect);
    }

    const menuRef = React.useRef();

    return (
        <div>
            <Typography variant="caption" color="textSecondary" style={{position:'fixed', bottom: 0, width:'100%'}}>
            v{process.env.VERSION} 
          </Typography>
          <div>
            
            <AppBar position="static">
              <Toolbar variant="dense"> 
                <Typography variant="h6" style={{flexGrow:1}}>
                </Typography>
                <IconButton ref={menuRef} onClick={()=>handleOpen()} edge="start" color="inherit" aria-label="menu">
                  <MenuIcon/>
                </IconButton>
              </Toolbar>
            </AppBar>

            <Menu anchorEl={menuRef.current} open={showMenu} onClose={handleClose} keepMounted>
              <MenuItem onClick={()=>handleClose("/")}>Goals</MenuItem>
              <MenuItem onClick={()=>handleClose("/log")}>Log</MenuItem>
            </Menu>
          </div>

          <LinearProgress style={{visibility:refreshing ? undefined : 'hidden'}} />
        </div>
    )
}