import React, { Component } from 'react';
import LatestTestResultsList from './LatestTestResultsList'
import { getUrlParams } from '../common/routing/SearchFilter.js'

class TestList extends Component {
  constructor(props) {
    super(props);

    this.state = { 
      suiteName: '',
      latestTestResults: [],
      filter: this.props.filter
    }

    this.handleFilterChange = this.handleFilterChange.bind(this);
  }

  getData() {
    this.props.getTests(this.props.filter)
      .then(response => {
        this.setState({
          suiteName: response.SuiteName,
          latestTestResults: response.LatestTestResults
        })
      })
      .catch(error => console.log('Failed to fetch latest test results: ' + error))
  }

  componentDidMount() {
    this.getData();
    this.intervalId = setInterval(() => this.getData(), 20000);
  }

  componentWillUnmount() {
    clearInterval(this.intervalId);
  }

  handleFilterChange(filter) {
    this.setState({
      filter: filter
    })
    window.location.search = getUrlParams(filter);
    this.getData();
  }  

  render() {
    return (
      <div>
        <LatestTestResultsList 
          suiteName={this.state.suiteName}
          testResults={this.state.latestTestResults}
          filter={this.state.filter}
          onFilterChange={this.handleFilterChange} />
      </div>
    );
  }
}

export default TestList;
