import React, { Component } from 'react';

import SuiteResults from './home/SuiteResults'
import ZigNetApi from './api/ZigNetApi'


class App extends Component {
  constructor() {
    super();
    this.zigNetApi = new ZigNetApi(process.env.REACT_APP_API_BASE_URL + 'api/');
  }

  render() {
    return (
      <div>
        <h1>ZigNet</h1>
        <SuiteResults suiteResults={this.zigNetApi.getLatestSuiteResults()} />
      </div>
    );
  }
}

export default App;
