import React, { Component } from 'react';
import { Route, Switch } from 'react-router-dom'
import LatestSuiteResults from './home/LatestSuiteResults'
import LatestTestResults from './suite/LatestTestResults'
import BadRoute from './common/BadRoute'
import ZigNetApi from './api/ZigNetApi'

class App extends Component {

  render() {
    let zigNetApi = new ZigNetApi(process.env.REACT_APP_API_BASE_URL + 'api/');
    return (
      <div>
        <Switch>
          <Route exact path="/" render={(props) => <LatestSuiteResults {...props} zigNetApi={zigNetApi} />} />
          <Route path='/:suiteId' render={(props) => <LatestTestResults {...props} zigNetApi={zigNetApi} />} />
          <Route component={BadRoute} />
        </Switch>
      </div>
    );
  }
}

export default App;