import React, { Component } from 'react';
import SuiteResults from './SuiteResults'

class LatestSuiteResults extends Component {
  constructor(props) {
    super(props);

    if (this.props.location.search.indexOf('group=true') !== -1)
      this.suiteResultsGrouped = true;
    else
      this.suiteResultsGrouped = false;

    this.zigNetApi = this.props.zigNetApi;
    this.state = { latestSuiteResults: [] }
  }

  componentDidMount() {
    this._getLatestSuiteResults(this.suiteResultsGrouped);
    this.intervalId = setInterval(() => this._getLatestSuiteResults(this.suiteResultsGrouped), 10000);
  }

  componentWillUnmount() {
    clearInterval(this.intervalId);
  }

  _getLatestSuiteResults(suiteResultsGrouped) {
    this.zigNetApi.getLatestSuiteResults(suiteResultsGrouped)
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
        <SuiteResults suiteResultsGrouped={this.suiteResultsGrouped} suiteResults={this.state.latestSuiteResults} />
      </div>
    );
  }
}

export default LatestSuiteResults;
