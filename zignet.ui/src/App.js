import React, { Component } from 'react';

import SuiteResults from './home/SuiteResults'
import ZigNetApi from './api/ZigNetApi'


class App extends Component {
  constructor() {
    super();
    
    this.zigNetApi = new ZigNetApi(process.env.REACT_APP_API_BASE_URL + 'api/');
    this.state = { latestSuiteResults: [] }
  }

  componentDidMount() {
    this._getLatestSuiteResults();
    this.intervalId = setInterval(() => this._getLatestSuiteResults(), 5000);
  }

  _getLatestSuiteResults() {
    this.zigNetApi.getLatestSuiteResults()
      .then(response => {
        this.setState({
          latestSuiteResults: response
        })
      })
      .catch(error => alert(error));    
  }

  render() {
    return (
      <div>
        <h1>ZigNet</h1>
        <SuiteResults suiteResults={this.state.latestSuiteResults} />
      </div>
    );
  }
}

export default App;