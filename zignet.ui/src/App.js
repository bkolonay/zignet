import React, { Component } from 'react';
import { Route, Switch } from 'react-router-dom'
import LatestSuiteResults from './home/LatestSuiteResults'
import SuiteResult from './suiteResult/SuiteResult'
import BadRoute from './common/BadRoute'

class App extends Component {

  render() {
    return (
      <div>
        <h1 className="text-center">ZigNet</h1>
        <Switch>
          <Route exact path="/" 
                 render={() => <LatestSuiteResults zigNetApi={this.props.zigNetApi} />} 
          />
          <Route path='/suiteResult/:suiteResultId' component={SuiteResult}/>
          <Route component={BadRoute} />
        </Switch>
      </div>
    );
  }
}

export default App;