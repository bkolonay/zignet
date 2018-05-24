import React, { Component } from 'react';
import { Route, Switch } from 'react-router-dom'
import LatestSuiteResultsRouter from './home/LatestSuiteResultsRouter'
import LatestTestResultsRouter from './suite/LatestTestResultsRouter'
import BadRoute from './common/BadRoute'

class App extends Component {

  render() {
    return (
      <div>
        <Switch>
          <Route exact path="/" render={(props) => 
            <LatestSuiteResultsRouter queryString={props.location.search} />} 
          />
          <Route path='/:suiteId' render={(props) =>
            <LatestTestResultsRouter queryString={props.location.search} suiteId={props.match.params.suiteId} />}
          />
          <Route component={BadRoute} />
        </Switch>
      </div>
    );
  }
}

export default App;