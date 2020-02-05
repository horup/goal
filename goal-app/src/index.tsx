import * as React from 'react';
import * as ReactDom from 'react-dom';
import { GoalsApi, Configuration, Goal } from '../autogenerated';
import { Container, Typography, TextField, Button, LinearProgress, Snackbar } from '@material-ui/core';

declare var process;
let api = new GoalsApi(
    new Configuration({
        basePath:process.env.BASE_PATH != null ? process.env.BASE_PATH  : location.origin
    })
);

if('serviceWorker' in navigator) 
{
    navigator.serviceWorker
    .register('./sw.ts')
    .then(function() { console.log('Service Worker Registered'); });
}

const GoalRow = ({goal, onClick}:{goal:Goal, onClick:(goal:Goal)=>any})=>{
  return (
    <div>
      <Typography onClick={()=>onClick != null ? onClick(goal) : undefined} color="textSecondary" style={{float:'left', cursor:'pointer', width:'32px'}}>
        {goal.id}
      </Typography>
      <Typography  align="left">
        {goal.description}
      </Typography>
    </div>
  )
}

const Index = ()=>
{
    const [goals, setGoals] = React.useState<Goal[]>([]);
    const [description, setDescription] = React.useState<string>("");
    const [refreshing, setRefreshing] = React.useState<boolean>(false);
    const [showAlert, setShowAlert] = React.useState<boolean>(false);
    const refresh = async ()=>
    {
      try
      {
        setRefreshing(true);
        let e = await api.api1GoalsGet();
        setGoals(e);
        setDescription("");
        setRefreshing(false);
      }
      catch(e)
      {
        if (e.status == 401)
        {
          window.location.href = "/login";
        }
        setShowAlert(true);
      }
      finally
      {
        setRefreshing(false);
      }
    }

    React.useEffect(()=>
    {
      refresh();
      window.onfocus = ()=>refresh();
    }, [])

    const addGoal = async ()=>
    {
      await api.api1GoalsPost({
        postGoal:
        {
          description:description
        }
      });

      await refresh();
    }

    const deleteGoal = async (goal:Goal)=>
    {
      if (confirm("Do you wish to delete this goal?"))
      {
        await api.api1GoalsDelete({
          body:goal.id
        });

        await refresh();
      }
    };

    return (
        <div>
          <LinearProgress style={{visibility:refreshing ? undefined : 'hidden'}} />
          <Container maxWidth="sm">
            <div style={{ marginTop: 24}}>
              <div style={{display:'flex', marginBottom:24}}>
                <TextField onKeyDown={(e)=>e.keyCode == 13 ? addGoal() : undefined} autoComplete="off" InputProps ={{readOnly:refreshing}} onChange={(e)=>setDescription(e.target.value)} value={description} style={{width:'100%', marginRight:16}} id="standard-basic" label="Description" />
                <Button disabled={description == "" || refreshing} onClick={()=>addGoal()} variant="outlined">Insert</Button>
              </div>
              {goals.map((g, i)=>
              {
                return (
                  <GoalRow onClick={deleteGoal} goal={g} key={i}/>
                )
              })}
            </div>
          </Container>
          <Snackbar open={showAlert} autoHideDuration={3000} onClose={()=>setShowAlert(false)} >
            <Typography color="error">
              Failure to get goals!
            </Typography>
          </Snackbar>
          <Typography variant="caption" color="textSecondary" style={{position:'fixed', bottom: 0, width:'100%'}}>
            v{process.env.VERSION} 
          </Typography>
        </div>
      );
}


ReactDom.render(<Index/>, document.getElementById("main"));