import React, { Component } from 'react';
import { Route, Switch } from 'react-router-dom'
import HomeRouter from './home/HomeRouter'
import TestResultsRouter from './testList/TestResultsRouter'
import BadRoute from './common/BadRoute'

class App extends Component {

  render() {
    return (
      <div>
        <Switch>
          <Route exact path="/" render={(props) => 
            <HomeRouter queryString={props.location.search} />} />
          <Route path='/:suiteId' render={(props) =>
            <TestResultsRouter queryString={props.location.search} suiteId={props.match.params.suiteId} />} />
          <Route component={BadRoute} />
        </Switch>
      </div>
    );
  }
}

export default App;