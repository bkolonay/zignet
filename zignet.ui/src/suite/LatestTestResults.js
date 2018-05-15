import React, { Component } from 'react';
import LatestTestResultsList from './LatestTestResultsList'

class LatestTestResults extends Component {
  constructor(props) {
    super(props);

    if (this.props.location.search.indexOf('group=true') !== -1)
      this.testResultsGrouped = true;
    else
      this.testResultsGrouped = false;

    this.zigNetApi = this.props.zigNetApi;
    this.state = { 
      suiteName: '',
      latestTestResults: []
    }
    this.suiteId = this.props.match.params.suiteId;
  }

  componentDidMount() {
    this._getLatestTestResultsForSuite(this.suiteId, this.testResultsGrouped);
    this.intervalId = setInterval(() => this._getLatestTestResultsForSuite(this.suiteId, this.testResultsGrouped), 10000);
  }

  componentWillUnmount() {
    clearInterval(this.intervalId);
  }

  _getLatestTestResultsForSuite(suiteId, testResultsGrouped) {
    this.zigNetApi.getLatestTestResultsForSuite(suiteId, testResultsGrouped)
      .then(response => {
        this.setState({
          suiteName: response.SuiteName,
          latestTestResults: response.LatestTestResults
        })
      })
      .catch(error => alert(error));
  }

  render() {
    return (
      <div>
        <LatestTestResultsList suiteName={this.state.suiteName} testResults={this.state.latestTestResults} />
      </div>
    );
  }
}

export default LatestTestResults;
