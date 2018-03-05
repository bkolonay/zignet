import React, { Component } from 'react';

import SuiteResults from './home/SuiteResults'

class App extends Component {
  constructor(props) {
    super(props);

    this.zigNetApi = this.props.zigNetApi;
    this.state = { latestSuiteResults: [] }
  }

  componentDidMount() {
    this._getLatestSuiteResults();
    this.intervalId = setInterval(() => this._getLatestSuiteResults(), 5000);
  }

  componentWillUnmount() {
    clearInterval(this.intervalId);
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
        <h1 className="text-center">ZigNet</h1>
        <SuiteResults suiteResults={this.state.latestSuiteResults} />
      </div>
    );
  }
}

export default App;