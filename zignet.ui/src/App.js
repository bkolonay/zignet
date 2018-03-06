import React, { Component } from 'react';

import { Route, Switch } from 'react-router-dom'

import LatestSuiteResults from './home/LatestSuiteResults'
import SuiteResult from './suiteResult/SuiteResult'
import BadRoute from './common/BadRoute'

class App extends Component {
  constructor({match}) {
    super();
    this.match = match;
  }


  render() {
    return (
      <div>
        <h1 className="text-center">ZigNet</h1>
        <Switch>
          <Route exact path="/" 
                 render={(props) => <LatestSuiteResults zigNetApi={this.props.zigNetApi} />} 
           />
          <Route path='/suiteResult/:suiteResultId' component={SuiteResult}/>
          <Route component={BadRoute} />
        </Switch>
      </div>
    );
  }
}

export default App;